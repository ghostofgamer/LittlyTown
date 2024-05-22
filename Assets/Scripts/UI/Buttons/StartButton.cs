using Dragger;
using ItemContent;
using ItemPositionContent;
using SaveAndLoad;
using UI.Buttons;
using UnityEngine;


public class StartButton : AbstractButton
{
    private const string LastActiveMap = "LastActiveMap";
    private const string Map = "Map";
    
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Transform _container;
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private Save _save;
    [SerializeField] private Item[] _items;

    private int _selectMap = 1;

    protected override void OnClick()
    {
        DeactivateItems();
        _itemDragger.ClearAll();

        foreach (var itemPosition in _itemPositions)
        {
            itemPosition.ClearingPosition();
        }

        foreach (var item in _items)
        {
            item.SetInitialPrice();
        }
        
        _mapGenerator.Generation();
        _itemDragger.SwitchOn();
        _save.SetData(LastActiveMap, _selectMap);
        _save.SetData(Map + 1, _selectMap);
    }

    public void DeactivateItems()
    {
        Transform[] children = _container.GetComponentsInChildren<Transform>(true);

        if (children.Length > 0)
        {
            foreach (Transform child in children)
            {
                if (child != _container.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}