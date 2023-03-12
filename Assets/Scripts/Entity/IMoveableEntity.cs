using System.Collections.Generic;
using UnityEngine;

public interface IMoveableEntity
{
    public void Move(WorldEntity targetWorldEntity);
    
    public void Move(Vector3Int destination);
}