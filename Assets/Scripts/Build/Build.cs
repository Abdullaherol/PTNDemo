using System.Collections.Generic;
using UnityEngine;

public class Build : WorldEntity,IBuild
{
    private List<Vector3Int> _tilePositions;
    
    public List<Vector3Int> GetTilePositions()
    {
        return _tilePositions;
    }

    public void SetTilePositions(List<Vector3Int> positions)
    {
        _tilePositions = positions;
    }
}