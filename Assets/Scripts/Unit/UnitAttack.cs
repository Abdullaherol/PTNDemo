using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour//Unit attack system seperated from base class and it has some specific features 
{
    private WorldEntity _target;
    private float _damage;
    private float _fireRate;

    private UnitMovement _movement;
    private BuildManager _buildManager;
    private Unit _unit;

    private bool _isAttacking;
    private bool _isMoving;

    private float _currentTime;

    // Initializes the UnitAttack component with the given parameters.
    public void Initialize(WorldEntity worldEntity)
    {
        _movement = GetComponent<UnitMovement>();
        _buildManager = BuildManager.Instance;
        
        var entity = worldEntity.entity;

        _unit = worldEntity as Unit;

        _damage = entity.damage;
        _fireRate = entity.fireRate;

        _movement.OnMovementStateChange += OnMovementStateChange;
    }

    // Handles state changes in the UnitMovement component.
    private void OnMovementStateChange(bool isMoving)
    {
        _isMoving = isMoving;

        if (!_isMoving && _target != null)
        {
            Attack(_target);
        }
    }

    // Attacks the given WorldEntity.
    public void Attack(WorldEntity target)
    {
        if (_isMoving || target == null) return;

        _target = target;

        if (!CheckCanAttack())
        {
            _movement.Move(target);
            return;
        }

        _target.health.OnEntityDestroy += OnTargetDestroy;

        if (_isAttacking)
        {
            StopAttack();
        }

        StartAttack();
    }

    // Handles the OnEntityDestroy event of the target WorldEntity.
    private void OnTargetDestroy()
    {
        StopAttack();
    }

    // Starts the attack coroutine.
    private void StartAttack()
    {
        _isAttacking = true;
        _target.health.OnEntityDestroy += OnTargetDestroy;

        StartCoroutine(nameof(AttackOnTarget));
    }

    // Stops the attack coroutine.
    public void StopAttack()
    {
        _isAttacking = false;
        _target = null;
        StopCoroutine(nameof(AttackOnTarget));
    }

    // The attack coroutine.
    private IEnumerator AttackOnTarget()
    {
        var attackTime = 1 / _fireRate;

        while (_target != null && !_isMoving)
        {
            yield return new WaitForSeconds(attackTime);

            _target.health.TakeDamage(_damage);
        }

        _isAttacking = false;
    }

    // Checks whether the unit can attack the target.
    private bool CheckCanAttack()
    {
        return !_isMoving && _buildManager.IsCloseToWalkablePosition(_unit, _unit.gridPosition, _target);
    }
}