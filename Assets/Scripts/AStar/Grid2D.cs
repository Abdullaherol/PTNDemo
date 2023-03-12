using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour
{
    private Vector2Int worldSize;
    private Node2D[,] grid;

    private WorldManager _worldManager;
    private BuildManager _buildManager;

    private void Start()
    {
        _worldManager = WorldManager.Instance;

        _buildManager = BuildController.Instance.buildManager;

        _buildManager.OnBuildsModified += OnBuildsModified;

        worldSize = _worldManager.worldSize;

        CreateGrid(_buildManager.GetAllBuildPositions());
    }

    //Handle OnBUildsModified, then refresh grid nodes
    private void OnBuildsModified()
    {
        RefreshGrid();
    }

    //Create grid nodes with obstaclePositions
    private void CreateGrid(List<Vector3Int> obstaclePositions)
    {
        grid = new Node2D[worldSize.x, worldSize.y];

        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                Vector3Int gridPos = new Vector3Int(x, y, 0);

                grid[x, y] = new Node2D(obstaclePositions.Contains(gridPos), gridPos);
            }
        }
    }

    private void RefreshGrid()
    {
        CreateGrid(_buildManager.GetAllBuildPositions());
    }

    //Get node's Neighbors
    public List<Node2D> GetNeighbors(Node2D node)
    {
        List<Node2D> neighbors = new List<Node2D>();

        if (node.gridPosition.x >= 0 && node.gridPosition.x < worldSize.x && node.gridPosition.y + 1 >= 0 &&
            node.gridPosition.y + 1 < worldSize.y)
            neighbors.Add(grid[node.gridPosition.x, node.gridPosition.y + 1]);

        if (node.gridPosition.x >= 0 && node.gridPosition.x < worldSize.x && node.gridPosition.y - 1 >= 0 &&
            node.gridPosition.y - 1 < worldSize.y)
            neighbors.Add(grid[node.gridPosition.x, node.gridPosition.y - 1]);

        if (node.gridPosition.x + 1 >= 0 && node.gridPosition.x + 1 < worldSize.x && node.gridPosition.y >= 0 &&
            node.gridPosition.y < worldSize.y)
            neighbors.Add(grid[node.gridPosition.x + 1, node.gridPosition.y]);

        if (node.gridPosition.x - 1 >= 0 && node.gridPosition.x - 1 < worldSize.x && node.gridPosition.y >= 0 &&
            node.gridPosition.y < worldSize.y)
            neighbors.Add(grid[node.gridPosition.x - 1, node.gridPosition.y]);

        return neighbors;
    }

    //Get node by gridPosition
    public Node2D GetNodeByPosition(Vector3Int position)
    {
        return grid[position.x, position.y];
    }
}