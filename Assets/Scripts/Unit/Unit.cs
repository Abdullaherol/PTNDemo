using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : WorldEntity, IUnit
{
    [SerializeField] private UnitAttack _attack;
    [SerializeField] private UnitMovement _movement;

    private void Start()
    {
        ConfigureHealth();
        
        _movement.Configure(entity);
        _attack.Configure(entity, _movement);
    }

    public void Shoot(WorldEntity target)
    {
        _attack.Attack(target);
    }

    public void Move(List<Vector3Int> path)
    {
        _movement.Move(path);
    }
}