using System.Collections;
using System.Collections.Generic;
using CountersContent;
using Dragger;
using ItemContent;
using PossibilitiesContent;
using Unity.VisualScripting;
using UnityEngine;
using Wallets;

public class StartMap : MonoBehaviour
{
    [SerializeField] private Initializator _initializator;
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Storage _storage;
    [SerializeField] private MovesKeeper _movesKeeper;
    [SerializeField] private GoldWallet _goldWallet;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private Item[] _items;
    [SerializeField] private Possibilitie[] _possibilities;
    [SerializeField] private PossibilitiesCounter[] _possibilitiesCounters;
    [SerializeField] private PackageLittleTown _packageLittleTown;
    [SerializeField] private BonusesStart _bonusesStart;
    [SerializeField] private MapGenerator _mapGenerator;

    private Transform[] _children;
    
    public void StartCreate()
    {
        _initializator.FillLists();
        DeactivateItems();
        _itemDragger.ClearAll();
        _storage.ClearItem();
        _movesKeeper.ClearAllHistory();
        _goldWallet.SetInitialvalue();
        _scoreCounter.ResetScore();

        foreach (var itemPosition in _initializator.ItemPositions)
        {
            itemPosition.ClearingPosition();
        }

        foreach (var item in _items)
        {
            item.SetInitialPrice();
        }

        foreach (var possibility in _possibilities)
        {
            possibility.SetStartPrice();
        }

        foreach (var possibilityCounter in _possibilitiesCounters)
        {
            possibilityCounter.SetCount();

            if (_packageLittleTown.IsActive)
                possibilityCounter.IncreaseCount(_packageLittleTown.Amount);
        }

        if (_packageLittleTown.IsActive)
            _packageLittleTown.Activated();


        _mapGenerator.TestGeneration(_initializator.Territories,_initializator.FinderPositions);
        _itemDragger.SwitchOn();
        _bonusesStart.ApplyBonuses();
    }

    public void DeactivateItems()
    {
        
        foreach (Transform child in _initializator.CurrentMap.RoadsContainer.transform)
        {
            child.gameObject.SetActive(false);
        }
        
        /*foreach (var itemPosition in _initializator.ItemPositions)
        {
            itemPosition.gameObject.SetActive(false);
        }
        foreach (var territory in _initializator.Territories)
        {
            territory.gameObject.SetActive(false);
        }*/
        
        
        /*Transform[] children = _initializator.Container.GetComponentsInChildren<Transform>(true);
         _children = _initializator.Container.GetComponentsInChildren<Transform>(true);

        if (children.Length > 0)
        {
            foreach (Transform child in children)
            {
                if (child != _initializator.Container.transform)
                    child.gameObject.SetActive(false);
            }
        }*/
    }
}