using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;

[System.Serializable]
public class InformationProductionItem : MonoBehaviour//Information panel production part item. with this item we can produce new units.
{
    private Entity _entity;

    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    private UnitManager _unitManager;

    private RectTransform _rectTransform;

    public void Initialize(Entity entity,float size)
    {
        _entity = entity;
        _image.sprite = entity.image;

        _unitManager = UnitManager.Instance;
        
        _button.onClick.RemoveAllListeners();
        
        _button.onClick.AddListener(Produce);
        
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        Debug.Log(size);
        
        _rectTransform.sizeDelta = new Vector2(size, size);
    }

    public void Produce()
    {
        _unitManager.PlaceUnit(_entity);
    }
}