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
    
    public void TakeDamage(float damage)//Return live status
    {
        _health -= damage;

        OnHealthChange?.Invoke(_health);

        if (_health <= 0)
        {
            OnEntityDestroy?.Invoke();
        }
    }
}