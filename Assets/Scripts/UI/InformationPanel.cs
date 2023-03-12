using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : Singleton<InformationPanel>, ISelectionObserver
{
    [SerializeField] private TMPro.TextMeshProUGUI _entityNameText;
    [SerializeField] private Image _entityImage;
    [SerializeField] private Image _entityHealthImage;
    [SerializeField] private GameObject _productionContent;
    [SerializeField] private List<InformationProductionItem> _productionItems = new List<InformationProductionItem>();

    private WorldEntity _lastSelectedWorldEntity;

    private void Start()
    {
        SelectionManager.Instance.Attach(this);
    }

    private void RefreshUI(WorldEntity worldEntity)
    {
        if (worldEntity == null)
        {
            _entityNameText.text = null;
            _entityImage.gameObject.SetActive(false);
            _entityHealthImage.gameObject.SetActive(false);
            _productionContent.SetActive(false);
        }
        else
        {
            Entity entity = worldEntity.entity;
            
            _entityImage.gameObject.SetActive(true);
            _entityHealthImage.gameObject.SetActive(true);

            _entityNameText.text = entity.entityName;
            _entityImage.sprite = entity.image;


            if (!entity.isProductionBuild)
            {
                _productionContent.SetActive(false);
                return;
            }

            _productionContent.SetActive(true);

            for (int i = 0; i < _productionItems.Count; i++)//burayı ayır.
            {
                var productionItem = _productionItems[i];

                if (i < entity.productionUnits.Count)
                {
                    var unitEntity = entity.productionUnits[i];

                    productionItem.Initialize(unitEntity);

                    productionItem.gameObject.SetActive(true);
                }
                else
                {
                    productionItem.gameObject.SetActive(false);
                }
            }
        }
    }
    
    public void OnSelect(WorldEntity worldEntity)
    {
        if (_lastSelectedWorldEntity != null)
        {
            if (_lastSelectedWorldEntity != worldEntity)
            {
                _lastSelectedWorldEntity.health.OnHealthChange -= OnHealthChange;
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

    private void OnHealthChange(float health)
    {
        if (_lastSelectedWorldEntity == null) return;

        var maxHealth = _lastSelectedWorldEntity.entity.health;

        var percentage = health / maxHealth;

        _entityHealthImage.fillAmount = percentage;
    }

    private void UpdateManuelHealthBar()
    {
        var maxHealth = _lastSelectedWorldEntity.entity.health;

        var health = _lastSelectedWorldEntity.health.healthPoint;
        
        var percentage = health / maxHealth;
        
        _entityHealthImage.fillAmount = percentage;
    }
}