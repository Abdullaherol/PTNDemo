using System.Collections.Generic;
using UnityEngine;

public interface IMoveableEntity
{
    public void Move(List<Vector3Int> path);
}