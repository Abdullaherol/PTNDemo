using System;
using UnityEngine;

[RequireComponent(typeof(EntityHealth))]//Base class
public class WorldEntity : MonoBehaviour //Base class for each entity in grid
{
    public Entity entity;
    public EntityHealth health;

    public Vector3Int gridPosition;

    //Configure health point from entity
    protected void ConfigureHealth()
    {
        health.healthPoint = entity.health;
    }
}