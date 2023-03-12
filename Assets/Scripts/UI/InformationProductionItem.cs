using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InformationProductionItem : MonoBehaviour
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
        var selectionManager = SelectionManager.Instance.GetComponent<SelectionManager>();
        var selectedEntity = selectionManager.selectedEntity;

        var unit = _unitManager.ProduceUnit(_entity);
        
        MoveUnitProductionPosition(selectedEntity,unit);
    }

    private void MoveUnitProductionPosition(WorldEntity selectedEntity,WorldEntity unitEntity)
    {
        Build build = selectedEntity.GetComponent<Build>();

        var sizeOffset = (Vector3)unitEntity.entity.gridSize / 2;
        
        var position = build.gridPosition + selectedEntity.entity.productionOffset + sizeOffset;

        unitEntity.transform.position = position;
    }
}