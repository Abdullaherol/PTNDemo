using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    private WorldEntity _target;

    private float _damage;
    private float _fireRate;

    private UnitMovement _movement;
    private bool _onMove;

    private float _currentTime;

    public void Initialize(Entity entity, UnitMovement movement)
    {
        _damage = entity.damage;
        _fireRate = entity.fireRate;
        _movement = movement;

        _movement.OnMovementStateChange += OnMovementStateChange;
    }

    private void OnMovementStateChange(bool state)
    {
        _onMove = state;
    }

    public void Attack(WorldEntity target)
    {
        if (_onMove || target == null) return;

        _target = target;

        _target.health.OnEntityDestroy += OnTargetDestroy;

        StopAttack();
        StartAttack();
    }

    private void OnTargetDestroy()
    {
        StopAttack();
    }

    private void StartAttack()
    {
        StartCoroutine(nameof(AttackOnTarget));
    }

    private void StopAttack()
    {
        StopCoroutine(nameof(AttackOnTarget));
    }

    private IEnumerator AttackOnTarget()
    {
        var attackTime = 1 / _fireRate;

        while (_target != null && !_onMove)
        {
            yield return new WaitForSeconds(attackTime);
            
            _target.health.TakeDamage(_damage);
        }
    }
}