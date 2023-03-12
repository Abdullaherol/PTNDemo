using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour,IMoveableEntity
{
    public delegate void OnMovementStateChangeHandler(bool state);
    public event OnMovementStateChangeHandler OnMovementStateChange;
    
    private List<Vector3Int> _pathPositions;
    private float _speed;
    private Vector3Int _gridSize;

    private float _currentTime;
    private int _pathIndex;
    
    public void Move(List<Vector3Int> path)
    {
        _pathPositions = path;
        
        StopMove();
        StartMove();
    }

    private void StartMove()
    {
        if(_pathPositions == null && _pathPositions.Count == 0) return;
        
        OnMovementStateChange?.Invoke(true);
        
        StartCoroutine(nameof(MoveOnPath));
    }

    private void StopMove()
    {
        OnMovementStateChange?.Invoke(false);
        StopCoroutine(nameof(MoveOnPath));
    }

    public void Initialize(Entity entity)
    {
        _speed = entity.speed;
        _gridSize = entity.gridSize;
    }

    private IEnumerator MoveOnPath()
    {
        var moveTime = 1 / _speed;

        var offset = (Vector3)_gridSize / 2;

        while (_pathPositions.Count - 1 > _pathIndex && _pathPositions.Count > 1)
        {
            var currentPosition = _pathPositions[_pathIndex] + offset;

            var destination = _pathPositions[_pathIndex + 1] + offset;

            while (_currentTime <= moveTime)
            {
                _currentTime += Time.deltaTime;

                float t = _currentTime / moveTime;

                transform.position = Vector3.Lerp(currentPosition, destination, t);
            
                yield return  new WaitForEndOfFrame();
            }
            _currentTime = 0;
            
            _pathIndex++;
        }
        
        OnMovementStateChange?.Invoke(false);
    }
}