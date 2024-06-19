using CountersContent;
using Dragger;
using ItemPositionContent;
using SaveAndLoad;
using UI.Buttons;
using UnityEngine;

public class ContinueButton : AbstractButton
{
    private const string Map = "Map";
    private const string CurrentRecordScore = "CurrentRecordScore";
    
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Save _save;
    [SerializeField] private ItemsStorage _itemsStorage;
    [SerializeField] private PossibilitiesCounter[] _possibilitiesCounters;
    [SerializeField] private PackageLittleTown _packageLittleTown;
    [SerializeField] private Initializator _initializator;
    [SerializeField]private MovesKeeper _moveKeeper;
    [SerializeField] private MapActivator _mapActivator;
    [SerializeField]private VisualItemsDeactivator _visualItemsDeactivator;
    [SerializeField]private ScoreCounter _scoreCounter;
    [SerializeField] private Load _load;
    [SerializeField] private GoldCounter _goldCounter;
    [SerializeField] private TurnEnvironment _turnEnvironment;
    [SerializeField] private GameObject _lightHouse;
    
    private int _startValue;
    
    protected override void OnClick()
    {
        _save.SetData(Map, _initializator.Index);
        _initializator.FillLists();
        _itemsStorage.LoadDataInfo();
        // _mapActivator.ChangeActivityMaps();
        _itemDragger.SetItem(_itemsStorage.SelectSaveItem, _itemsStorage.SelectSaveItem.ItemPosition);
        // _itemDragger.SelectedObject.gameObject.SetActive(true);
        _itemDragger.SwitchOn();
        _visualItemsDeactivator.SetPositions(_initializator.ItemPositions);
        _moveKeeper.LoadHistoryData();
        _scoreCounter.SetCurrentScore(_load.Get(CurrentRecordScore + _initializator.Index, _startValue));
        _goldCounter.CheckIncome();
        _turnEnvironment.SetEnvironment();
        
        _lightHouse.SetActive(_initializator.CurrentMap.IsWaterTilePresent);
        
        if (_packageLittleTown.IsActive)
        {
            foreach (var possibilitiesCounter in _possibilitiesCounters)
            {
                Debug.Log("Contiue");
                possibilitiesCounter.IncreaseCount(_packageLittleTown.Amount);
            }
                

            _packageLittleTown.Activated();
        }
    }
}