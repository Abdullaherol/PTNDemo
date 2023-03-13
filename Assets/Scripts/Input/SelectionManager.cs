using System;
using UnityEngine;
using Utility;

public class SelectionManager : SelectionSubject //manager for in game board. it provides selection and targeting 
{
    [HideInInspector] public WorldEntity selectedEntity;
    [HideInInspector] public WorldEntity targetEntity;

    private Camera _mainCamera;
    private BuildController _buildController;
    private BuildManager _buildManager;

    private bool _OnBuildMode;

    private Vector3Int _lastMouseGridPosition;

    private void Start()
    {
        _buildController = BuildController.Instance;
        _buildController.OnBuildModeChange += OnBuildModeChange;

        _buildManager = _buildController.buildManager;

        _mainCamera = Camera.main;
    }

    //Handles OnBuildModeChange event
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

        if (Input.GetMouseButtonDown(1) && selectedEntity != null) //Move
        {
            if (!selectedEntity.entity.isUnit) return;

            SelectTarget();
        }
    }

    //Selection entity with left mouse click
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

    //Selection target with right mouse click
    private void SelectTarget()
    {
        targetEntity = null;

        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        Unit unit = selectedEntity.GetComponent<Unit>();

        if (hit.collider == null)
        {
            _lastMouseGridPosition = GetMouseGridPosition();
            unit.Move(_lastMouseGridPosition);

            return;
        }

        if (!hit.transform.TryGetComponent<WorldEntity>(out WorldEntity worldEntity)) return;

        targetEntity = worldEntity;

        if (targetEntity == null)
        {
            unit.Move(_lastMouseGridPosition);
            return;
        }

        unit.Attack(targetEntity);
    }

    //Return mouse world position
    private Vector3 GetMouseWorldPosition()
    {
        var mousePosition = Input.mousePosition;
        var worldPoint = _mainCamera.ScreenToWorldPoint(mousePosition);

        return worldPoint;
    }

    //Return mouse grid position
    private Vector3Int GetMouseGridPosition()
    {
        var worldPosition = GetMouseWorldPosition();

        return new Vector3Int((int)worldPosition.x, (int)worldPosition.y, 0);
    }

    //Checks is mouse over ui
    private bool CheckMouseOverUI()
    {
        return UIUtility.IsMouseOverUI();
    }
}