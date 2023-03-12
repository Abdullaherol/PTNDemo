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

    public void Initialize(Entity entity)
    {
        _entity = entity;
        _image.sprite = entity.image;

        _unitManager = UnitManager.Instance;
        
        _button.onClick.RemoveAllListeners();
        
        _button.onClick.AddListener(Produce);
    }

    public void Produce()
    {
        _unitManager.PlaceUnit(_entity);
    }
}