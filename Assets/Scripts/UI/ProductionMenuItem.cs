using System;
using UnityEngine;
using UnityEngine.UI;

public class ProductionMenuItem : MonoBehaviour//In Infinite scroll view/Production menu, script for each menu item control.
{
    public Entity entity;

    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    private BuildController _buildController;

    private void Start()
    {
        _buildController = BuildController.Instance;

        _button.onClick.AddListener(Click);
    }

    private void Click()
    {
        _buildController.Build(entity);
    }

    public void Configure(Entity item)
    {
        entity = item;

        _image.sprite = entity.image;
    }
}