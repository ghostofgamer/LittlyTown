using CountersContent;
using Dragger;
using SaveAndLoad;
using UI.Buttons;
using UnityEngine;

public class ContinueButton : AbstractButton
{
    private const string Map = "Map";
    
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Save _save;
    [SerializeField] private ItemsStorage _itemsStorage;
    [SerializeField] private PossibilitiesCounter[] _possibilitiesCounters;
    [SerializeField] private PackageLittleTown _packageLittleTown;
    [SerializeField] private Initializator _initializator;
    [SerializeField]private MovesKeeper _moveKeeper;
    [SerializeField] private MapActivator _mapActivator;
    
    protected override void OnClick()
    {
        _save.SetData(Map, _initializator.Index);
        _initializator.FillLists();
        _itemsStorage.LoadDataInfo();
        _mapActivator.ChangeActivityMaps();
        _itemDragger.SetItem(_itemsStorage.SelectSaveItem, _itemsStorage.SelectSaveItem.ItemPosition);
        // _itemDragger.SelectedObject.gameObject.SetActive(true);
        _itemDragger.SwitchOn();
        _moveKeeper.LoadHistoryData();
        if (_packageLittleTown.IsActive)
        {
            foreach (var possibilitiesCounter in _possibilitiesCounters)
                possibilitiesCounter.IncreaseCount(_packageLittleTown.Amount);

            _packageLittleTown.Activated();
        }
    }
}