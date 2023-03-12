using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class PathFinding : Singleton<PathFinding>
{
    private Grid2D grid;

    private Node2D startNode, targetNode;

    private void Start()
    {
        grid = GetComponent<Grid2D>();
    }

    //Calculate path by A* algorithm
    public List<Vector3Int> FindPath(Vector3Int startPosition, Vector3Int targetPosition)
    {
        startNode = grid.GetNodeByPosition(startPosition);
        targetNode = grid.GetNodeByPosition(targetPosition);

        List<Node2D> discoveredNodes = new List<Node2D>();
        HashSet<Node2D> visitedNodes = new HashSet<Node2D>();
        discoveredNodes.Add(startNode);

        while (discoveredNodes.Count > 0)
        {
            Node2D node = discoveredNodes[0];
            for (int i = 0; i < discoveredNodes.Count; i++)
            {
                if (discoveredNodes[i].fCost <= node.fCost)
                {
                    if (discoveredNodes[i].hCost < node.hCost)
                    {
                        node = discoveredNodes[i];
                    }
                }
            }

            discoveredNodes.Remove(node);
            visitedNodes.Add(node);

            if (node == targetNode)
            {
               return GetPath(startNode, targetNode);
            }

            foreach (var neighbor in grid.GetNeighbors(node))
            {
                if (neighbor.obstacle || visitedNodes.Contains(neighbor))
                {
                    continue;
                }

                int costToNeighbor = node.gCost + GetDistance(node, neighbor);

                if (costToNeighbor < neighbor.gCost || !discoveredNodes.Contains(neighbor))
                {
                    neighbor.gCost = costToNeighbor;
                    neighbor.hCost = GetDistance(neighbor,targetNode);
                    neighbor.parent = node;

                    if (!discoveredNodes.Contains(neighbor))
                    {
                        discoveredNodes.Add(neighbor);
                    }
                }
            }
        }

        return new List<Vector3Int>();
    }

    //Get path by calculated points then reverse and return path
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

    //Calculate distance between first and second node
    private int GetDistance(Node2D firstNode, Node2D secondNode)
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