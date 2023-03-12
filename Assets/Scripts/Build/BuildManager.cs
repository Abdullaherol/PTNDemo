using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector3 = System.Numerics.Vector3;

public class BuildManager : MonoBehaviour
{
    public delegate void OnBuildsModifiedHandler();
    public event OnBuildsModifiedHandler OnBuildsModified;
    
    private List<Build> _builds = new List<Build>();

    public List<Vector3Int> GetAllBuildPositions()
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        foreach (var build in _builds)
        {
            foreach (var tilePosition in build.GetTilePositions())
            {
                positions.Add(tilePosition);
            }
        }

        return positions;
    }
    //
    // public bool IsBuilding(Vector3Int position, out Build build)
    // {
    //     foreach (var b in _builds)
    //     {
    //         var tiles = b.GetTilePositions();
    //
    //         if (tiles.Contains(position))
    //         {
    //             build = b;
    //             return true;
    //         }
    //     }
    //
    //     build = null;
    //     return false;
    // }

    public bool CanPlaceWorldEntity(Vector3Int gridPosition, WorldEntity worldEntity)
    {
        var worldEntityPoints = CalculateWorldEntityGridPositions(gridPosition, worldEntity);
        var buildsPoints = GetAllBuildPositions();

        foreach (var worldEntityPoint in worldEntityPoints)
        {
            if (buildsPoints.Contains(worldEntityPoint))
            {
                return false;
            }
        }

        return true;
    }

    private List<Vector3Int> CalculateWorldEntityGridPositions(Vector3Int gridPosition, WorldEntity worldEntity)
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        var entity = worldEntity.entity;

        var size = entity.gridSize;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var worldEntityPosition = gridPosition + new Vector3Int(x, y, 0);

                positions.Add(worldEntityPosition);
            }
        }

        if (entity.isProductionBuild)
        {
            positions.Add(gridPosition + entity.productionOffset);
        }

        return positions;
    }

    public void PlaceBuild(Vector3Int gridPosition, WorldEntity worldEntity)
    {
        Build build = worldEntity.GetComponent<Build>();
        build.SetTilePositions(CalculateWorldEntityGridPositions(gridPosition, worldEntity));

        build.gridPosition = gridPosition;
        
        OnBuildsModified?.Invoke();

        _builds.Add(build);
    }

    public List<Vector3Int> GetWalkablePositions(Build build)
    {
        var size = build.entity.gridSize;
        var gridPosition = build.gridPosition;

        List<Vector3Int> positions = new List<Vector3Int>();

        var finalIndex = size.y - 1;

        for (int y = 0; y < size.y; y++)
        {
            if (y % finalIndex == 0)
            {
                for (int i = -1; i < size.x + 1; i++)
                {
                    positions.Add(gridPosition + new Vector3Int(i, y, 0));
                }
            }
            else
            {
                positions.Add(gridPosition + new Vector3Int(-1, y, 0));
                positions.Add(gridPosition + new Vector3Int(size.x, y, 0));
            }
        }

        return positions;
    }

    public Vector3Int GetClosestWalkablePosition(Vector3Int startPosition,Build build)
    {
        var walkablePositions = GetWalkablePositions(build);

        var minDistance = walkablePositions.Min(x => Vector3Int.Distance(startPosition, x));

        return walkablePositions.First(x => Vector3Int.Distance(startPosition,x) == minDistance);
    }
}