using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductionMenuController : MonoBehaviour, IScrollHandler//Production menu controller. it provides infinite scroll view and it has object pooling system.
{
    [SerializeField] private GameObject _productionMenuItemPrefab;
    [SerializeField] private Transform content;

    [SerializeField] private float _menuItemHeight;
    [SerializeField] private float _menuItemPadding;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private Vector2Int _menuGridSize;

    [SerializeField] private List<Entity> _entities = new List<Entity>();

    private List<ProductionMenuItem> _items = new List<ProductionMenuItem>();
    private List<Entity> _unusedEntities = new List<Entity>();//pool

    private float _contentHeight;
    private float _contentWidth;

    private void Start()
    {
        var rect = GetComponent<RectTransform>().rect;

        _contentHeight = rect.height;
        _contentWidth = rect.width;

        for (int y = 0; y < _menuGridSize.y; y++) //pool objects spawning
        {
            for (int x = 0; x < _menuGridSize.x; x++)
            {
                var child = Instantiate(_productionMenuItemPrefab, content);
                var index = y * _menuGridSize.x + x;
                var entity = _entities[index];

                var item = child.GetComponent<ProductionMenuItem>();
                item.Configure(entity);

                var radius = _menuItemHeight / 2;
                var posY = (-radius - _menuItemPadding) * (1 + y * 2);

                var posX = (_menuItemPadding * (x + 1)) + (radius * (1 + x * 2));

                posX += (_contentWidth - radius * 4 - _menuItemPadding * 2) / 2; //For dynamic aspect ratio, it center item

                child.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, posY, 0);

                _items.Add(item);
            }
        }

        var poolIndex = _menuGridSize.x * _menuGridSize.y;

        for (int i = poolIndex; i < _entities.Count; i++) //For object pooling we store other entities
        {
            var entity = _entities[i];

            _unusedEntities.Add(entity);
        }
    }

    public void OnScroll(PointerEventData eventData) //We detect scroll delta and move content
    {
        var scrollDelta = eventData.scrollDelta.y * _scrollSpeed;

        content.transform.position += new Vector3(0, scrollDelta, 0);

        RepositionMenuItems(scrollDelta > 0);
    }

    private void RepositionMenuItems(bool up) //Control items thresholds and move up or down, then assign entity
    {
        var outDistance = _contentHeight / 2 + _menuItemHeight / 2;

        var rowFirstIndex = (up) ? 0 : (_menuGridSize.x * _menuGridSize.y) - 2;
        var rowSecondIndex = (up) ? 1 : (_menuGridSize.x * _menuGridSize.y) - 1;

        var rowFirstItem = _items[rowFirstIndex];
        var rowSecondItem = _items[rowSecondIndex];

        var rowFirstPosition = rowFirstItem.transform.position;
        var rowSecondPosition = rowSecondItem.transform.position;

        var distance = Vector3.Distance(rowFirstPosition, transform.position);

        if (distance < outDistance) return;

        var referenceIndex = (up) ? (_menuGridSize.x * _menuGridSize.y) - 1 : 0;

        var referenceItem = _items[referenceIndex];

        var referencePosition = referenceItem.transform.position;

        var targetHeight = referencePosition.y + ((up) ? -(_menuItemPadding + _menuItemHeight) : (_menuItemPadding + _menuItemHeight));

        rowFirstItem.transform.position = new Vector3(rowFirstPosition.x, targetHeight, 0);
        rowSecondItem.transform.position = new Vector3(rowSecondPosition.x, targetHeight, 0);

        if (up)
        {
            ProcessUp(rowFirstItem,rowSecondItem);
        }
        else
        {
            ProcessDown(rowFirstItem,rowSecondItem);
        }
    }
    
    private void ProcessUp(ProductionMenuItem rowFirstItem,ProductionMenuItem rowSecondItem)
    {
        var firstEntity = _unusedEntities[0];
        var secondEntity = _unusedEntities[1];

        _unusedEntities.Add(rowFirstItem.entity);
        _unusedEntities.Add(rowSecondItem.entity);

        rowFirstItem.Configure(firstEntity);
        rowSecondItem.Configure(secondEntity);

        _unusedEntities.Remove(firstEntity);
        _unusedEntities.Remove(secondEntity);

        _items.Remove(rowFirstItem);
        _items.Add(rowFirstItem);

        _items.Remove(rowSecondItem);
        _items.Add(rowSecondItem);
    }

    private void ProcessDown(ProductionMenuItem rowFirstItem,ProductionMenuItem rowSecondItem)
    {
        var lastFirstEntity = _unusedEntities[^1];
        var lastSecondEntity = _unusedEntities[^2];

        _unusedEntities.Insert(0, rowSecondItem.entity);
        _unusedEntities.Insert(0, rowFirstItem.entity);

        rowSecondItem.Configure(lastFirstEntity);
        rowFirstItem.Configure(lastSecondEntity);

        _unusedEntities.Remove(lastFirstEntity);
        _unusedEntities.Remove(lastSecondEntity);

        _items.Remove(rowFirstItem);
        _items.Insert(0, rowFirstItem);

        _items.Remove(rowSecondItem);
        _items.Insert(0, rowSecondItem);
    }
}