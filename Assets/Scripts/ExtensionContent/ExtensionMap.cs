using System;
using System.Collections;
using System.Collections.Generic;
using ItemContent;
using ItemPositionContent;
using MapsContent;
using SaveAndLoad;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExtensionMap : MonoBehaviour
{
    private const string ExtensionTerritory = "ExtensionTerritory";
    private const string WaterTile = "WaterTile";

    [SerializeField] private Map[] _extensionMaps;
    [SerializeField] private RoadGenerator _roadGenerator;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private Merger _merger;
    [SerializeField] private Territory[] _extensionTerritories;
    [SerializeField] private Initializator _initializator;
    [SerializeField] private Save _save;
    [SerializeField] private Load _load;
    [SerializeField] private Transform _transform;
    [SerializeField] private ExtensionMapMovement _extensionMapMovement;

    private List<Territory> _targetTerritories = new List<Territory>();
    private List<Territory> _extensionFilterTerritories = new List<Territory>();
    private List<FinderPositions> _targetFinderPositions = new List<FinderPositions>();
    private List<ItemPosition> _targetItemPositions = new List<ItemPosition>();

    private int _index = 0;
    private Map _currentMap;

    public event Action IndexChanged;
    
    private void OnEnable()
    {
        _merger.ItemMergered += Extension;
    }

    private void OnDisable()
    {
        _merger.ItemMergered -= Extension;
    }

    private void Start()
    {
        // _index = _load.Get(ExtensionTerritory + _currentMap.Index, 0);
    }

    private void Extension(Item item)
    {
        if (!_initializator.CurrentMap.IsMapExpanding)
            return;

        if (_index >= _currentMap.ExpandingTerritories.Length)
        {
            return;
        }

        _save.SetData(ExtensionTerritory + _currentMap.Index, _index + 1);

        Debug.Log("РАСШИРЯЮЩАЯСЯ КАРТА ИНДЕКС " + _currentMap.Index);

        _currentMap.ExpandingTerritories[_index].gameObject.SetActive(true);
        _currentMap.ExpandingTerritories[_index].PositionActivation();


        Debug.Log(" ИНДЕКС " + _index);

        FinderPositions[] finderPositionsInTerritory =
            _currentMap.ExpandingTerritories[_index].gameObject.GetComponentsInChildren<FinderPositions>(true);

        foreach (FinderPositions finder in finderPositionsInTerritory)
        {
            if (!_targetFinderPositions.Contains(finder))
            {
                _targetFinderPositions.Add(finder);
            }
        }

        ItemPosition[] itemPositions =
            _currentMap.ExpandingTerritories[_index].gameObject.GetComponentsInChildren<ItemPosition>(true);

        foreach (var itemPos in itemPositions)
        {
            if (!itemPos.enabled)
                continue;
            
            Debug.Log("ПОЗИЦИИ " + itemPos);
        }
        
        Debug.Log(" _currentMap " + _currentMap.name);
        Debug.Log(" ___________________________________ ");

        foreach (ItemPosition itemPosition in itemPositions)
        {
            if (!itemPosition.enabled)
                continue;

            if (!_targetItemPositions.Contains(itemPosition))
            {
                _targetItemPositions.Add(itemPosition);
            }
        }

        foreach (var itemPos in _targetItemPositions)
        {
            Debug.Log("ПОЗИЦИИ " + itemPos);
        }

        /*foreach (var finderPosition in _targetFinderPositions)
        {
            if (finderPosition.gameObject.activeSelf == true)
                finderPosition.FindNeighbor();
        }*/

        _initializator.SetPositions(_targetItemPositions);


        _roadGenerator.TestGeneration(_targetItemPositions, _initializator.CurrentMap.RoadsContainer,
            _initializator.CurrentMap);

        _index++;
        IndexChanged?.Invoke();
        _extensionMapMovement.ChangePosition(_index, _currentMap.Mover);
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
        _extensionFilterTerritories = new List<Territory>();

        Territory[] territories = map.GetComponentsInChildren<Territory>(true);

        foreach (var territory in territories)
        {
            if (territory.IsExpanding)
            {
                _extensionFilterTerritories.Add(territory);
            }
        }

        int amount = _load.Get(ExtensionTerritory + map.Index, 0);
        _extensionMapMovement.SetPosition(amount, map.Mover);
/*
Debug.Log("AMOUNT " + amount);
Debug.Log("_extensionFilterTerritories " + _extensionFilterTerritories.Count);*/

        for (int i = 0; i < amount; i++)
        {
            _extensionFilterTerritories[i].SetOpened();
        }

        foreach (var territory in territories)
        {
            if (territory.IsExpanding && !territory.IsOpened)
            {
                continue;
            }

            if (!_targetTerritories.Contains(territory) && !territory.IsExpanding ||
                !_targetTerritories.Contains(territory) && territory.IsOpened)
            {
                // Debug.Log("territory " + territory.name);
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

        if (map.IsWaterRandom)
        {
            int index = _load.Get(WaterTile, 0);

            if (index > 0)
            {
                map.RandomPositionWaters[index].SetWater();
            }
            else
            {
                int randomIndex = Random.Range(1, _targetItemPositions.Count);
                map.RandomPositionWaters[randomIndex].SetWater();
                _save.SetData(WaterTile, randomIndex);
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

    public void ResetMap(Map map, List<Territory> territory, List<ItemPosition> itemPositions,
        List<FinderPositions> finderPositions, List<Territory> extensionTerritory)
    {
        _index = 0;
        // _save.SetData(ExtensionTerritory, 0);
        // Debug.Log("ОШИЬБКА");
        _targetTerritories = territory;
        _targetFinderPositions = finderPositions;
        _targetItemPositions = itemPositions;
        _extensionFilterTerritories = extensionTerritory;
        _extensionMapMovement.ResetPosition(_transform);
    }

    public void ContinueMap(List<Territory> territory, List<ItemPosition> itemPositions,
        List<FinderPositions> finderPositions, List<Territory> extensionTerritory)
    {
        _targetTerritories = territory;
        _targetFinderPositions = finderPositions;
        _targetItemPositions = itemPositions;
        _extensionFilterTerritories = extensionTerritory;
    }

    public void RandomWater(Map map)
    {
        foreach (var itemPosition in map.RandomPositionWaters)
        {
            itemPosition.ResetWater();
        }

        int randomIndex = Random.Range(1, map.RandomPositionWaters.Length);
        map.RandomPositionWaters[randomIndex].SetWater();
        _save.SetData(WaterTile, randomIndex);
    }

    public void SetMap(Map map)
    {
        _currentMap = map;
        _index = _load.Get(ExtensionTerritory + _currentMap.Index, 0);
        Debug.Log("SETMAP " + _currentMap.name);
    }
}