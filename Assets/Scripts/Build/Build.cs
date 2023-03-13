using System;
using System.Collections.Generic;
using UnityEngine;

public class Build : WorldEntity,IBuild//Base build class, each build world entity is build
{
    private List<Vector3Int> _tilePositions;

    private void Start()
    {
        ConfigureHealth();
    }

    //Return tile positions
    public List<Vector3Int> GetTilePositions()
    {
        return _tilePositions;
    }

    //Set tile positions
    public void SetTilePositions(List<Vector3Int> positions)
    {
        _tilePositions = positions;
    }
}