using System;
using System.Collections;
using System.Collections.Generic;
using ItemContent;
using ItemPositionContent;
using MapsContent;
using UnityEngine;

public class ExtensionMap : MonoBehaviour
{
    [SerializeField] private Map[] _extensionMaps;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private Merger _merger;
    [SerializeField] private Territory[] _extensionTerritories;
    [SerializeField] private Initializator _initializator;

    private List<Territory> _targetTerritories = new List<Territory>();
    private List<FinderPositions> _targetFinderPositions = new List<FinderPositions>();
    private List<ItemPosition> _targetItemPositions = new List<ItemPosition>();

    private int _index = 0;
    
    private void OnEnable()
    {
        _merger.ItemMergered += Extension;
    }

    private void OnDisable()
    {
        _merger.ItemMergered -= Extension;
    }

    private void Extension(Item item)
    {
        if (_index > 0)
            return;
        
        _extensionTerritories[_index].gameObject.SetActive(true);
        _extensionTerritories[_index].PositionActivation();
        

        FinderPositions[] finderPositionsInTerritory =
            _extensionTerritories[0].gameObject.GetComponentsInChildren<FinderPositions>(true);

        foreach (FinderPositions finder in finderPositionsInTerritory)
        {
            if (!_targetFinderPositions.Contains(finder))
            {
                _targetFinderPositions.Add(finder);
            }
        }

        ItemPosition[] itemPositions = _extensionTerritories[0].gameObject.GetComponentsInChildren<ItemPosition>(true);


        foreach (ItemPosition itemPosition in itemPositions)
        {
            if (!itemPosition.enabled)
                continue;

            if (!_targetItemPositions.Contains(itemPosition))
            {
                _targetItemPositions.Add(itemPosition);
            }
        }

        foreach (var finderPosition in _targetFinderPositions)
        {
            if (finderPosition.gameObject.activeSelf == true)
                finderPosition.FindNeighbor();
        }

        _initializator.SetPositions(_targetItemPositions);
        
        
        _roadGenerator.TestGeneration(_targetItemPositions, _initializator.CurrentMap.RoadsContainer,
            _initializator.CurrentMap);

        _index += 1;
    }
    
    

    public void SearchMap(int index)
    {
        foreach (var map in _extensionMaps)
        {
            if (map.Index == index)
            {
                ShowMap(map);
            }
        }
    }

    private void ShowMap(Map map)
    {
        _targetTerritories = new List<Territory>();
        _targetFinderPositions = new List<FinderPositions>();
        _targetItemPositions = new List<ItemPosition>();

        Territory[] territories = map.GetComponentsInChildren<Territory>(true);

        foreach (var territory in territories)
        {
            if (territory.IsExpanding)
            {
                continue;
            }

            if (!_targetTerritories.Contains(territory) && !territory.IsExpanding)
            {
                _targetTerritories.Add(territory);
            }
        }

        foreach (Territory territory in _targetTerritories)
        {
            FinderPositions[] finderPositionsInTerritory =
                territory.gameObject.GetComponentsInChildren<FinderPositions>(true);


            foreach (FinderPositions finder in finderPositionsInTerritory)
            {
                if (!_targetFinderPositions.Contains(finder))
                {
                    _targetFinderPositions.Add(finder);
                }
            }
        }

        foreach (Territory territory in _targetTerritories)
        {
            ItemPosition[] itemPositions = territory.gameObject.GetComponentsInChildren<ItemPosition>(true);


            foreach (ItemPosition itemPosition in itemPositions)
            {
                if (!itemPosition.enabled)
                    continue;

                if (!_targetItemPositions.Contains(itemPosition))
                {
                    _targetItemPositions.Add(itemPosition);
                }
            }
        }

        foreach (var territory in _targetTerritories)
        {
            // territory.PositionDeactivation();

            territory.gameObject.SetActive(false);

            // _roadGenerator.DeactivateRoad();
        }

        _mapGenerator.TestShowMap(_targetTerritories, _targetFinderPositions, map.RoadsContainer,
            _targetItemPositions, map.StartItems, map);
    }
}