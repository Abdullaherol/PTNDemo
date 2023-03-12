using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Entity/Create a new Entity")]
public class Entity : ScriptableObject //Main scriptable object for base world entites
{
    [Header("General")] public string entityName;
    public Sprite image;
    public GameObject prefab;
    public float health;
    public Vector3Int gridSize;

    [Space, Header("Production")] public bool isProductionBuild;
    public Vector3Int productionOffset;
    public List<Entity> productionUnits = new List<Entity>();

    [Space, Header("Unit")] public bool isUnit;
    public float damage;
    public float speed;
    public float fireRate;
}