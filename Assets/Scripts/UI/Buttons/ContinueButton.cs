using CountersContent;
using EnvironmentContent;
using InitializationContent;
using ItemPositionContent;
using Keeper;
using SaveAndLoad;
using UnityEngine;
using UpgradesContent;

namespace UI.Buttons
{
    public class ContinueButton : AbstractButton
    {
        private const string Map = "Map";
        private const string CurrentRecordScore = "CurrentRecordScore";

        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private Save _save;
        [SerializeField] private GameStorage _gameStorage;
        [SerializeField] private PossibilitiesCounter[] _possibilitiesCounters;
        [SerializeField] private PackageLittleTown _packageLittleTown;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private MovesKeeper _moveKeeper;
        [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private Load _load;
        [SerializeField] private GoldCounter _goldCounter;
        [SerializeField] private TurnEnvironment _turnEnvironment;
        [SerializeField] private GameObject _lightHouse;

        private int _startValue;

        protected override void OnClick()
        {
            _save.SetData(Map, _initializator.Index);
            _initializator.FillLists();
            _gameStorage.LoadDataInfo();
            _itemKeeper.SetItem(_gameStorage.SelectSaveItem, _gameStorage.SelectSaveItem.ItemPosition);
            _itemKeeper.SwitchOn();
            _visualItemsDeactivator.SetPositions(_initializator.ItemPositions);
            _moveKeeper.LoadHistoryData();
            _scoreCounter.SetCurrentScore(_load.Get(CurrentRecordScore + _initializator.Index, _startValue));
            _goldCounter.OnCheckIncome();
            _turnEnvironment.SetEnvironment(_initializator.CurrentMap.gameObject);
            _lightHouse.SetActive(_initializator.CurrentMap.IsWaterTilePresent);

            if (_packageLittleTown.IsActive)
            {
                foreach (PossibilitiesCounter possibilitiesCounter in _possibilitiesCounters)
                    possibilitiesCounter.OnIncreaseCount(_packageLittleTown.Amount);

                _packageLittleTown.Activated();
            }
        }
    }
}