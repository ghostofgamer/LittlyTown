using System;
using System.Collections.Generic;
using ItemPositionContent;
using MapsContent;
using SaveAndLoad;
using UnityEngine;

public class Initializator : MonoBehaviour
{
    [SerializeField] private Transform[] _environments;
    [SerializeField] private ChooseMap _chooseMap;
    [SerializeField] private Load _load;
    
    private List<ItemPosition> _currentItemPositions = new List<ItemPosition>();
    private List<Territory> _currentTerritories = new List<Territory>();
    private List<FinderPositions> _currentFinderPositions = new List<FinderPositions>();
    private Transform _container;
    private Map _currentMap;
    private int _index;

    public int Index => _index;

    public int AmountMaps;

    public event Action IndexChanged; 
    
    public Transform Container => _container;

    public List<FinderPositions> FinderPositions => _currentFinderPositions;
    
    public List<Territory> Territories => _currentTerritories;
    
    public List<ItemPosition> ItemPositions => _currentItemPositions;
        
    public Map CurrentMap => _currentMap;

    private void Awake()
    {
        AmountMaps = _environments.Length;
    }

    private void OnEnable()
    {
        _chooseMap.MapChanged += SetIndex;
    }

    private void OnDisable()
    {
        _chooseMap.MapChanged -= SetIndex;
    }

    public void SetIndex(int index)
    {
        _index = index;
        IndexChanged?.Invoke();
    }

    public void FillLists()
    {
        _currentItemPositions = new List<ItemPosition>();
        _currentTerritories = new List<Territory>();
        _currentFinderPositions = new List<FinderPositions>();

        _container = _environments[_index];
        _currentMap = _environments[_index].GetComponent<Map>();
        Territory[] territories = _environments[_index].GetComponentsInChildren<Territory>(true);
        FinderPositions[] finderPositionScripts = _environments[_index].GetComponentsInChildren<FinderPositions>(true);
        ItemPosition[] itemPositions = _environments[_index].GetComponentsInChildren<ItemPosition>(true);

        foreach (var territory in territories)
        {
            if (!_currentTerritories.Contains(territory))
            {
                _currentTerritories.Add(territory);
            }
        }

        foreach (FinderPositions finderPositionScript in finderPositionScripts)
        {
            if (!_currentFinderPositions.Contains(finderPositionScript))
            {
                _currentFinderPositions.Add(finderPositionScript);
            }
        }

        foreach (var itemPosition in itemPositions)
        {
            if (!itemPosition.enabled)
                continue;

            if (!_currentItemPositions.Contains(itemPosition))
            {
                _currentItemPositions.Add(itemPosition);
            }
        }
    }
}