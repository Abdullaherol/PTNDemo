using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour//You can control camera.
{
    [Header("Movement")] [SerializeField] private float _moveSpeed;

    [Space, Header("Scroll & Size")] [SerializeField]
    private float _maxSize;

    [SerializeField] private float _minSize;
    [SerializeField] private float _scrollSpeed;

    private WorldManager _worldManager;
    private Camera _cameraComponent;

    void Start()
    {
        _worldManager = WorldManager.Instance;
        _cameraComponent = GetComponent<Camera>();
        
        GoCenterPosition();
    }

    void Update()
    {
        //Move and Scroll
        MoveCamera();
        ScrollCamera();
        
        //Check
        CheckCameraBounds();
        CheckCameraSize();
    }

    private void MoveCamera()//Camera movement
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontal, vertical, 0) * _moveSpeed;
    }

    private void ScrollCamera()//Camera orthographicSize with mouse scroll
    {
        var scroll = Input.mouseScrollDelta.y;

        _cameraComponent.orthographicSize += scroll * _scrollSpeed;
    }

    private void CheckCameraBounds()//Checking camera bound with camera height and width
    {
        var worldSize = _worldManager.worldSize;
        
        var height = 2f * _cameraComponent.orthographicSize;
        var width = height * _cameraComponent.aspect;

        var radiusH = height / 2;
        var radiusW = width / 2;

        var position = transform.position;

        position.x = Mathf.Clamp(position.x, 0 + radiusW, worldSize.x - radiusW);
        position.y = Mathf.Clamp(position.y, 0 + radiusH, worldSize.y - radiusH);

        transform.position = position;
    }

    private void CheckCameraSize()//Checking camera size with orthographicSize
    {
        var size = _cameraComponent.orthographicSize;

        size = Mathf.Clamp(size, _minSize, _maxSize);

        _cameraComponent.orthographicSize = size;
    }

    private void GoCenterPosition()//Set camera position to center world.
    {
        var worldSize = _worldManager.worldSize;

        var centerPosition = new Vector3(worldSize.x, worldSize.y, transform.position.z) / 2;

        transform.position = centerPosition;
    }
}