using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitManager : Singleton<UnitManager>//Manager class for units, it provides producing and placing units in grid.
{
    private List<Unit> _units = new List<Unit>();

    private FactoryManager _factoryManager;

    private void Start()
    {
        _factoryManager = FactoryManager.Instance;
    }

    private WorldEntity ProduceUnit(Entity entity)
    {
        return _factoryManager.GetWorldEntity(entity);
    }

    public void DestroyUnit(Unit unit)
    {
        _units.Remove(unit);
        _factoryManager.ReturnWorldEntity(unit);
    }

    public List<Vector3Int> GetAllUnitPositions(Unit exclusiveUnit)
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        foreach (var unit in _units)
        {
            if (exclusiveUnit == unit) continue;
            positions.AddRange(CalculateWorldEntityGridPositions(unit.gridPosition, unit));
        }

        return positions;
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
    
    public void PlaceUnit(Entity entity)
    {
        var selectionManager = SelectionManager.Instance.GetComponent<SelectionManager>();
        var selectedEntity = selectionManager.selectedEntity;

        var worldEntity = ProduceUnit(entity) as Unit;

        worldEntity.health.healthPoint = worldEntity.entity.health;
        
        _units.Add(worldEntity);
        
        MoveUnitProductionPosition(selectedEntity,worldEntity);
    }

    private void MoveUnitProductionPosition(WorldEntity selectedEntity,WorldEntity unitEntity)
    {
        Build build = selectedEntity.GetComponent<Build>();
        
        var sizeOffset = (Vector3)unitEntity.entity.gridSize / 2;
        var gridPosition = build.gridPosition + selectedEntity.entity.productionOffset;
        var position = gridPosition + sizeOffset;

        unitEntity.transform.position = position;
        unitEntity.gridPosition = gridPosition;
    }
}