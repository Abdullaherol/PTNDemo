using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour, IMoveableEntity//Unit movement system seperated from base class and it has some specific features 
{
    public event Action<bool> OnMovementStateChange;

    private List<Vector3Int> _pathPositions = new List<Vector3Int>();
    private float _speed;
    private Vector3Int _gridSize;
    private int _pathIndex;

    private Unit _unit;

    private PathFinding _pathFinding;
    private BuildManager _buildManager;

    private bool _isMoving;

    // Move the unit to the specified world entity
    public void Move(WorldEntity worldEntity)
    {
        var destination = _buildManager.GetClosestWalkablePosition(_unit, _unit.gridPosition, worldEntity);
        SetPath(_pathFinding.FindPath(_unit.gridPosition, destination));
        MoveUnit();
    }

    // Move the unit to the specified destination
    public void Move(Vector3Int destination)
    {
        SetPath(_pathFinding.FindPath(_unit.gridPosition, destination));
        MoveUnit();
    }

    // Set the path that the unit will follow
    private void SetPath(List<Vector3Int> path)
    {
        _pathPositions = path;
        _pathIndex = 0;
    }

    // Start moving the unit along the current path
    private void MoveUnit()
    {
        if (_isMoving || _pathPositions == null || _pathPositions.Count == 0) return;
        _isMoving = true;
        OnMovementStateChange?.Invoke(_isMoving);
        StartCoroutine(MoveOnPath());
    }

    // Coroutine to move the unit along the path
    private IEnumerator MoveOnPath()
    {
        var moveTime = 1 / _speed;
        var offset = (Vector3)_gridSize / 2;

        while (_pathIndex < _pathPositions.Count - 1)
        {
            var currentPosition = _pathPositions[_pathIndex] + offset;
            var destination = _pathPositions[_pathIndex + 1] + offset;

            for (float t = 0; t < moveTime; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(currentPosition, destination, t / moveTime);
                yield return null;
            }

            transform.position = destination;
            _unit.gridPosition = _pathPositions[++_pathIndex];
        }

        _isMoving = false;
        _pathPositions.Clear();
        OnMovementStateChange?.Invoke(_isMoving);
    }

    public void Initialize(WorldEntity worldEntity)
    {
        _unit = worldEntity as Unit;
        _speed = _unit.entity.speed;
        _gridSize = _unit.entity.gridSize;
        _pathFinding = PathFinding.Instance;
        _buildManager = BuildManager.Instance;
    }
}