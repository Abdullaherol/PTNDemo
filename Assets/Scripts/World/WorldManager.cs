using System;
using UnityEngine;

public class WorldManager : MonoBehaviour//World manager
{
    public static WorldManager Instance;
    
    public Vector2Int worldSize;

    private void Awake()
    {
        Instance = this;
    }
}