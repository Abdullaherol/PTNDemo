using System;
using UnityEngine;

[RequireComponent(typeof(EntityHealth))]
public class WorldEntity : MonoBehaviour
{
    public Entity entity;
    public EntityHealth health;

    protected void ConfigureHealth()
    {
        health.healthPoint = entity.health;
    }
}