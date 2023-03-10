using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGroundGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _groundTilemap;

    [SerializeField] private TileBase _groundTile;

    [SerializeField] private Color32 _baseColor;
    [SerializeField] private Color32 _offsetColor;

    private WorldManager _worldManager;

    private void Start()
    {
        _worldManager = WorldManager.Instance;
        
        GenerateGround();
    }

    private void GenerateGround()
    {
        var worldSize = _worldManager.worldSize;

        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                var position = new Vector3Int(x, y, 0);

                _groundTilemap.SetTile(position, _groundTile);
                
                _groundTilemap.SetTileFlags(position,TileFlags.None);

                var color = ((x + y) % 2 == 0) ? _baseColor : _offsetColor;

                _groundTilemap.SetColor(position, color);
            }
        }
    }
}