using System;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public delegate void OnHealthChangeHandler(float health);

    public delegate void OnEntityDestroyHandler();

    public event OnHealthChangeHandler OnHealthChange;
    public event OnEntityDestroyHandler OnEntityDestroy;

    private float _health;

    public float healthPoint
    {
        get => _health;
        set => _health = value;
    }

    public void TakeDamage(float damage) //Return live status
    {
        _health -= damage;

        OnHealthChange?.Invoke(_health);

        if (_health <= 0)
        {
            DestroyWorldEntity();
        }
    }

    public void DestroyWorldEntity()
    {
        OnEntityDestroy?.Invoke();
        
        var worldEntity = GetComponent<WorldEntity>();

        if (worldEntity is Build)
        {
            var build = worldEntity as Build;

            var buildManager = BuildManager.Instance;
            buildManager.DestroyBuild(build);
        }
        else
        {
            var unit = worldEntity as Unit;

            var unitManager = UnitManager.Instance;
            unitManager.DestroyUnit(unit);
        }
    }
}