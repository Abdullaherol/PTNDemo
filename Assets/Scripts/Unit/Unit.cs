using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : WorldEntity, IUnit//base unit class
{
    [SerializeField] private UnitAttack _attack;
    [SerializeField] private UnitMovement _movement;

    private void Start()
    {
        ConfigureHealth();
        
        _movement.Initialize(this);
        _attack.Initialize(this);
    }

    public void Attack(WorldEntity target)
    {
        _attack.Attack(target);
    }

    public void Move(Vector3Int destination)
    {
        _attack.StopAttack();
        _movement.Move(destination);
    }
}