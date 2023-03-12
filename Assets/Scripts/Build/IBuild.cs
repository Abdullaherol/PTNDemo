using System.Collections.Generic;
using UnityEngine;

public interface IBuild//Interface for Build
{
    public List<Vector3Int> GetTilePositions();
    public void SetTilePositions(List<Vector3Int> positions);
}