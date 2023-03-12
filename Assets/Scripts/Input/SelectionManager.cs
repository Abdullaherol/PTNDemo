using System;
using UnityEngine;
using Utility;

public class SelectionManager : SelectionSubject
{
    public WorldEntity selectedEntity;
    public WorldEntity targetEntity;

    private Camera _mainCamera;
    private BuildController _buildController;
    private BuildManager _buildManager;
    private PathFinding _pathFinding;

    private bool _OnBuildMode;

    private Vector3Int _lastMouseGridPosition;

    private void Start()
    {
        _pathFinding = PathFinding.Instance;

        _buildController = BuildController.Instance;
        _buildController.OnBuildModeChange += OnBuildModeChange;

        _buildManager = _buildController.buildManager;

        _mainCamera = Camera.main;
    }

    private void OnBuildModeChange(bool state)
    {
        _OnBuildMode = state;
    }

    private void Update()
    {
        if (_OnBuildMode || CheckMouseOverUI()) return;

        if (Input.GetMouseButtonDown(0))
        {
            SelectEntity();
        }

        if (Input.GetMouseButtonDown(1) && !_OnBuildMode && selectedEntity != null) //Move
        {
            if (!selectedEntity.entity.isUnit) return;

            SelectTarget();
            
            Unit unit = selectedEntity.GetComponent<Unit>();
            var startPosition = unit.transform.position.ConvertToVector3Int();

            if (targetEntity == null)
            {
                unit.Move(_pathFinding.FindPath(startPosition,_lastMouseGridPosition));
                
                return;
            }


            var targetPosition = targetEntity.transform.position.ConvertToVector3Int();

            if (targetEntity is Build)
            {
                Build build = targetEntity as Build;

                var walkablePosition = _buildManager.GetClosestWalkablePosition(startPosition, build);
                unit.Move(_pathFinding.FindPath(startPosition, walkablePosition));
            }
            else
            {
                unit.Move(_pathFinding.FindPath(startPosition, targetPosition));
            }
        }
    }

    private void SelectEntity()
    {
        selectedEntity = null;
        
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider == null)
        {
            Notify(null);

            return;
        }

        if (!hit.transform.TryGetComponent<WorldEntity>(out WorldEntity worldEntity)) ;

        selectedEntity = worldEntity;

        Notify(selectedEntity);
    }

    private void SelectTarget()
    {
        targetEntity = null;
        
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider == null)
        {
            _lastMouseGridPosition = GetMouseGridPosition();
            return;
        }

        if (!hit.transform.TryGetComponent<WorldEntity>(out WorldEntity worldEntity)) ;

        targetEntity = worldEntity;
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        var mousePosition = Input.mousePosition;
        var worldPoint = _mainCamera.ScreenToWorldPoint(mousePosition);

        return worldPoint;
    }

    private Vector3Int GetMouseGridPosition()
    {
        var worldPosition = GetMouseWorldPosition();

        return new Vector3Int((int)worldPosition.x, (int)worldPosition.y, 0);
    }


    private bool CheckMouseOverUI()
    {
        return UIUtility.IsMouseOverUI();
    }
}