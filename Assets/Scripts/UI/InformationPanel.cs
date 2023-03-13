using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : Singleton<InformationPanel>, ISelectionObserver//Information panel main script. it control all information panel components and updates
{
    [SerializeField] private TMPro.TextMeshProUGUI _entityNameText;
    [SerializeField] private Image _entityImage;
    [SerializeField] private Image _entityHealthImage;
    [SerializeField] private GameObject _productionParentContent;
    [SerializeField] private RectTransform _productionCItemContent;
    [SerializeField] private List<InformationProductionItem> _productionItems = new List<InformationProductionItem>();

    private WorldEntity _lastSelectedWorldEntity;

    private void Start()
    {
        SelectionManager.Instance.Attach(this);
        
        RefreshUI(null);
    }

    //Main refresh ui method
    private void RefreshUI(WorldEntity worldEntity)
    {
        if (worldEntity == null)
        {
            HidePanels();
        }
        else
        {
            Entity entity = worldEntity.entity;

            RefreshInformationPart(entity);

            if (!entity.isProductionBuild)
            {
                _productionParentContent.SetActive(false);
                return;
            }

            _productionParentContent.SetActive(true);

            RefreshProductionItems(entity);
        }
    }

    //Hide all panels
    private void HidePanels()
    {
        _entityNameText.text = null;
        _entityImage.gameObject.SetActive(false);
        _entityHealthImage.gameObject.SetActive(false);
        _productionParentContent.SetActive(false);
    }

    //Refresh information part
    private void RefreshInformationPart(Entity entity)
    {
        _entityImage.gameObject.SetActive(true);
        _entityHealthImage.gameObject.SetActive(true);

        _entityNameText.text = entity.entityName;
        _entityImage.sprite = entity.image;
    }

    //Refresh production items
    private void RefreshProductionItems(Entity entity)
    {
        var itemSize = _productionCItemContent.rect.size.y;
        
        for (int i = 0; i < _productionItems.Count; i++)
        {
            var productionItem = _productionItems[i];

            var image = productionItem.GetComponent<Image>();

            if (i < entity.productionUnits.Count)
            {
                var unitEntity = entity.productionUnits[i];

                image.enabled = true;
                    
                productionItem.Initialize(unitEntity,itemSize);

            }
            else
            {
                image.enabled = false;
            }
        }
    }

    //Handle OnSelect event
    public void OnSelect(WorldEntity worldEntity)
    {
        if (_lastSelectedWorldEntity != null)
        {
            if (_lastSelectedWorldEntity != worldEntity)
            {
                _lastSelectedWorldEntity.health.OnHealthChange -= OnHealthChange;

                _lastSelectedWorldEntity.health.OnEntityDestroy += OnSelectedEntityDestroyed;
            }
        }

        if (worldEntity != null)
        {
            _lastSelectedWorldEntity = worldEntity;

            _lastSelectedWorldEntity.health.OnHealthChange += OnHealthChange;

            UpdateManuelHealthBar();
        }

        RefreshUI(worldEntity);
    }

    //Handle OnHealthChange event
    private void OnHealthChange(float health)
    {
        if (_lastSelectedWorldEntity == null) return;

        var maxHealth = _lastSelectedWorldEntity.entity.health;

        var percentage = health / maxHealth;

        _entityHealthImage.fillAmount = percentage;
    }

    //Handle OnSelectedEntityDestroyed event
    private void OnSelectedEntityDestroyed()
    {
        OnSelect(null);
    }

    //Updates health bar by manuel
    private void UpdateManuelHealthBar()
    {
        var maxHealth = _lastSelectedWorldEntity.entity.health;

        var health = _lastSelectedWorldEntity.health.healthPoint;

        var percentage = health / maxHealth;

        _entityHealthImage.fillAmount = percentage;
    }
}