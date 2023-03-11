using System.Collections.Generic;
using UnityEngine;

public interface IBuild
{
    public List<Vector3Int> GetTilePositions();
    public void SetTilePositions(List<Vector3Int> positions);
}