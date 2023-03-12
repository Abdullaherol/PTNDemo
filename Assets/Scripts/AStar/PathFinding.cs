using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class PathFinding : Singleton<PathFinding>
{
    private Vector3Int startPosition, targetPosition;
    private Grid2D grid;

    private Node2D startNode, targetNode;

    private void Start()
    {
        grid = GetComponent<Grid2D>();
    }

    public List<Vector3Int> FindPath(Vector3Int startPosition, Vector3Int targetPosition)
    {
        this.startPosition = startPosition;
        this.targetPosition = targetPosition;

        startNode = grid.GetNodeByPosition(startPosition);
        targetNode = grid.GetNodeByPosition(targetPosition);

        List<Node2D> openSet = new List<Node2D>();
        HashSet<Node2D> closedSet = new HashSet<Node2D>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node2D node = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost <= node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                    {
                        node = openSet[i];
                    }
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
               return GetPath(startNode, targetNode);
            }

            foreach (var neighbor in grid.GetNeighbors(node))
            {
                if (neighbor.obstacle || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int costToNeighbor = node.gCost + GetDistance(node, neighbor);

                if (costToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = costToNeighbor;
                    neighbor.hCost = GetDistance(neighbor,targetNode);
                    neighbor.parent = node;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return new List<Vector3Int>();
    }

    private List<Vector3Int> GetPath(Node2D startNode, Node2D endNode)//Calculate path then reverse it.
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node2D currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.gridPosition);
            currentNode = currentNode.parent;
        }
        
        path.Add(this.startNode.gridPosition);

        path.Reverse();
        return path;
    }

    private int GetDistance(Node2D firstNode, Node2D secondNode)//Gets distance between first node and second nodes
    {
        int distanceX = Mathf.Abs(firstNode.gridPosition.x - secondNode.gridPosition.x);
        int distanceY = Mathf.Abs(firstNode.gridPosition.y - secondNode.gridPosition.y);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }

        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}