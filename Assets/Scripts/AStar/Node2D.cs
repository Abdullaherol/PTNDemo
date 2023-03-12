using System.Numerics;
using UnityEngine;

public class Node2D//For every signle point in the grid
{
    public int gCost;
    public int hCost;
    public bool obstacle;
    public Vector3Int gridPosition;
    public Node2D parent;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public Node2D(bool obstacle, Vector3Int gridPosition)
    {
        this.obstacle = obstacle;
        this.gridPosition = gridPosition;
    }
}