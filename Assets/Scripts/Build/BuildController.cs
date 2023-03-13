using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildController : Singleton<BuildController>//Control grid build placement
{
    public delegate void OnBuildModeChangeHandler(bool state);
    public event OnBuildModeChangeHandler OnBuildModeChange;

    public BuildManager buildManager;

    private WorldEntity _buildWorldEntity;
    private FactoryManager _factoryManager;
    private Camera _mainCamera;

    private Vector3 _lastMousePosition;
    private bool _onBuildMode;
    private bool _placeWrongAreTried;

    private void Start()
    {
        _mainCamera = Camera.main;

        _factoryManager = FactoryManager.Instance;
    }

    private void Update()
    {
        if (!CheckBuildMode()) return;

        TryExitBuildMode();

        MoveBuildIndicator();

        Place();
    }

    //Control placement and fire OnBuildModeChange event
    public void Place()
    {
        if (_placeWrongAreTried && Vector3.Distance(Input.mousePosition, _lastMousePosition) > 0)
        {
            if (_buildWorldEntity == null) return;

            _buildWorldEntity.GetComponent<SpriteRenderer>().color = Color.white;

            _placeWrongAreTried = false;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var gridPosition = GetMouseGridPosition();

            if (!buildManager.CanPlaceWorldEntity(gridPosition, _buildWorldEntity))
            {
                _buildWorldEntity.GetComponent<SpriteRenderer>().color = Color.red;
                
                _placeWrongAreTried = true;
                
                _lastMousePosition = Input.mousePosition;
                return;
            }

            buildManager.PlaceBuild(gridPosition, _buildWorldEntity);

            _onBuildMode = false;
            _buildWorldEntity = null;

            OnBuildModeChange?.Invoke(_onBuildMode);
        }
    }

    //Checks build mode
    private bool CheckBuildMode()
    {
        return _onBuildMode && !UIUtility.IsMouseOverUI();
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

    //Control build indicator position
    private void MoveBuildIndicator()
    {
        var worldPosition = GetMouseWorldPosition();

        var offset = (Vector3)_buildWorldEntity.entity.gridSize / 2;

        var position = new Vector3((int)worldPosition.x, (int)worldPosition.y, 0) + offset;

        _buildWorldEntity.transform.position = position;
    }

    //Check player exit
    private void TryExitBuildMode()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _onBuildMode = false;

            _factoryManager.ReturnWorldEntity(_buildWorldEntity);

            _buildWorldEntity = null;

            OnBuildModeChange?.Invoke(_onBuildMode);
        }
    }

    //Main method for build
    public void Build(Entity entity)
    {
        if (entity.isUnit)
        {
            if (_buildWorldEntity != null)
            {
                ReturnWorldEntity();
            }
        
            _onBuildMode = false;
            return;
        }

        if (_onBuildMode)
        {
            ReturnWorldEntity();
        }

        _onBuildMode = true;

        _buildWorldEntity = _factoryManager.GetWorldEntity(entity);

        OnBuildModeChange?.Invoke(_onBuildMode);
    }

    private void ReturnWorldEntity()
    {
        _factoryManager.ReturnWorldEntity(_buildWorldEntity);
        _buildWorldEntity = null;
    }
}