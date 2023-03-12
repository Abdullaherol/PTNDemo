using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    private List<Unit> _units = new List<Unit>();

    private FactoryManager _factoryManager;

    private void Start()
    {
        _factoryManager = FactoryManager.Instance;
    }

    public WorldEntity ProduceUnit(Entity entity)
    {
        return _factoryManager.GetWorldEntity(entity);
    }
}