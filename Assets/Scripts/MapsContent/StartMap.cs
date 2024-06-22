using System;
using CountersContent;
using EnvironmentContent;
using InitializationContent;
using ItemContent;
using ItemPositionContent;
using Keeper;
using PossibilitiesContent;
using SaveAndLoad;
using UnityEngine;
using Wallets;

namespace MapsContent
{
    public class StartMap : MonoBehaviour
    {
        private const string LastActiveMap = "LastActiveMap";
        private const string Map = "Map";
        private const string ActiveMap = "ActiveMap";

        [SerializeField] private Initializator _initializator;
        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private Storage[] _storages;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private Item[] _items;
        [SerializeField] private Possibilitie[] _possibilities;
        [SerializeField] private PossibilitiesCounter[] _possibilitiesCounters;
        [SerializeField] private PackageLittleTown _packageLittleTown;
        [SerializeField] private BonusesStart _bonusesStart;
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private MovesKeeper _moveKeeper;
        [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
        [SerializeField] private GoldCounter _goldCounter;
        [SerializeField] private TurnEnvironment _turnEnvironment;
        [SerializeField] private GameObject _lightHouse;
        [SerializeField] private Save _save;

        private Transform[] _children;
        private int _selectMap = 1;

        public event Action MapStarted;

        public void StartCreate()
        {
            _save.SetData(LastActiveMap, _selectMap);
            _save.SetData(Map, _initializator.Index);
            _save.SetData(ActiveMap + _initializator.Index, _selectMap);
        
            if (_initializator.Environments[_initializator.Index].GetComponent<Map>().IsMapExpanding)
                _initializator.ResetTerritory();
            else
                _initializator.FillLists();
        
            DeactivateItems();
            _visualItemsDeactivator.SetPositions(_initializator.ItemPositions);
            _itemKeeper.ClearAll();
            
            foreach (var storage in _storages)
                storage.ClearItem();
            
            _moveKeeper.LoadHistoryData();

            if (_initializator.CurrentMap.IsMapWithoutProfit)
            {
                _goldWallet.SetInitialValue();
                _goldWallet.DisableProfit();
            }
            else
            {
                _goldWallet.SetInitialValue();
                _goldWallet.EnableProfit();
            }

            _lightHouse.SetActive(_initializator.CurrentMap.IsWaterTilePresent);
            _goldCounter.CheckIncome();
            _scoreCounter.ResetScore();

            foreach (var itemPosition in _initializator.ItemPositions)
            {
                itemPosition.ClearingPosition();
                itemPosition.DisableRoad();
            }

            foreach (var item in _items)
                item.SetInitialPrice();
        
            foreach (var possibility in _possibilities)
                possibility.SetStartPrice();
        
            foreach (var possibilityCounter in _possibilitiesCounters)
            {
                possibilityCounter.SetCount();

                if (_packageLittleTown.IsActive)
                    possibilityCounter.OnIncreaseCount(_packageLittleTown.Amount);
            }

            if (_packageLittleTown.IsActive)
                _packageLittleTown.Activated();
        
            _turnEnvironment.SetEnvironment(_initializator.CurrentMap.gameObject);
            _mapGenerator.TestGeneration(_initializator.Territories, _initializator.FinderPositions,
                _initializator.CurrentMap.StartItems, _initializator.ItemPositions,
                _initializator.CurrentMap.ItemsContainer);
            _itemKeeper.SwitchOn();
            _bonusesStart.ApplyBonuses();
            MapStarted?.Invoke();
        }

        public void StartVisualCreate()
        {
            SetStartSettings();
            _mapGenerator.TestVisualGeneration(_initializator.Territories, _initializator.FinderPositions,
                _initializator.CurrentMap.StartItems, _initializator.ItemPositions,
                _initializator.CurrentMap.ItemsContainer);
        }


        public void SetStartSettings()
        {
            _save.SetData(Map, _initializator.Index);
            _initializator.FillLists();
            DeactivateItems();
            _visualItemsDeactivator.SetPositions(_initializator.ItemPositions);
            _itemKeeper.ClearAll();
        
            foreach (var storage in _storages)
                storage.ClearItem();
        
            _moveKeeper.LoadHistoryData();
            _goldWallet.SetInitialValue();
            _scoreCounter.ResetScore();

            foreach (var itemPosition in _initializator.ItemPositions)
                itemPosition.ClearingPosition();

            foreach (var item in _items)
                item.SetInitialPrice();

            foreach (var possibility in _possibilities)
                possibility.SetStartPrice();

            foreach (var possibilityCounter in _possibilitiesCounters)
            {
                possibilityCounter.SetCount();

                if (_packageLittleTown.IsActive)
                    possibilityCounter.OnIncreaseCount(_packageLittleTown.Amount);
            }

            if (_packageLittleTown.IsActive)
                _packageLittleTown.Activated();

            _itemKeeper.SwitchOn();
            _bonusesStart.ApplyBonuses();
        }

        private void DeactivateItems()
        {
            foreach (Transform child in _initializator.CurrentMap.RoadsContainer.transform)
                child.gameObject.SetActive(false);

            foreach (Transform child in _initializator.CurrentMap.ItemsContainer.transform)
                child.gameObject.SetActive(false);
        }
    }
}