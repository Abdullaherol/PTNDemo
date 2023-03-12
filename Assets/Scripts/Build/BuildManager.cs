using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector3 = System.Numerics.Vector3;

//Manage build,calculating build points and Place build
public class BuildManager : Singleton<BuildManager>
{
    public delegate void OnBuildsModifiedHandler();

    public event OnBuildsModifiedHandler OnBuildsModified;

    private List<Build> _builds = new List<Build>();

    private UnitManager _unitManager;

    private void Start()
    {
        _unitManager = UnitManager.Instance;
    }

    public List<Vector3Int> GetAllBuildPositions()
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        foreach (var build in _builds)
        {
            positions.AddRange(build.GetTilePositions());
        }

        return positions;
    }

    public bool CanPlaceWorldEntity(Vector3Int gridPosition, WorldEntity worldEntity)
    {
        var worldEntityPoints = CalculateWorldEntityGridPositions(gridPosition, worldEntity);
        var buildsPoints = GetAllBuildPositions();
        var unitPoints = _unitManager.GetAllUnitPositions(null);

        return worldEntityPoints.All(p => !buildsPoints.Contains(p) && !unitPoints.Contains(p));
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

        build.health.healthPoint = build.entity.health;

        OnBuildsModified?.Invoke();

        _builds.Add(build);
    }

    public List<Vector3Int> GetWalkablePositions(Unit walkUnit, WorldEntity worldEntity)
    {
        var unitPoints = _unitManager.GetAllUnitPositions(walkUnit);

        var size = worldEntity.entity.gridSize;
        var gridPosition = worldEntity.gridPosition;

        List<Vector3Int> positions = new List<Vector3Int>();

        var sizeY = (size.y + 1);
        
        for (int y = -1; y < sizeY; y++)
        {
            if ((y + 1) % sizeY == 0)
            {
                for (int i = -1; i < size.x + 1; i++)
                {
                    var position = gridPosition + new Vector3Int(i, y, 0);

                    if (!unitPoints.Contains(position))
                    {
                        positions.Add(position);
                    }
                }
            }
            else
            {
                var leftPosition = gridPosition + new Vector3Int(-1, y, 0);
                var rightPosition = gridPosition + new Vector3Int(size.x, y, 0);

                if (!unitPoints.Contains(leftPosition))
                {
                    positions.Add(leftPosition);
                }

                if (!unitPoints.Contains(rightPosition))
                {
                    positions.Add(rightPosition);
                }
            }
        }

        return positions;
    }

    public Vector3Int GetClosestWalkablePosition(Unit walkUnit, Vector3Int startPosition, WorldEntity worldEntity)
    {
        var walkablePositions = GetWalkablePositions(walkUnit, worldEntity);

        var minDistance = walkablePositions.Min(x => Vector3Int.Distance(startPosition, x));

        return walkablePositions.First(x => Vector3Int.Distance(startPosition, x) == minDistance);
    }

    public bool IsCloseToWalkablePosition(Unit walkUnit, Vector3Int position, WorldEntity worldEntity)
    {
        var walkablePosition = GetClosestWalkablePosition(walkUnit, position, worldEntity);

        return position == walkablePosition;
    }

    public void DestroyBuild(Build build)
    {
        _builds.Remove(build);

        var factoryManager = FactoryManager.Instance;

        factoryManager.ReturnWorldEntity(build);
    }
}