using System;
using System.Collections.Generic;
using ItemPositionContent;
using MapsContent;
using SaveAndLoad;
using UnityEngine;
using Random = UnityEngine.Random;

public class Initializator : MonoBehaviour
{
    private const string ExtensionTerritory = "ExtensionTerritory";
    private const string WaterTile = "WaterTile";

    [SerializeField] private Transform[] _environments;
    [SerializeField] private ChooseMap _chooseMap;
    [SerializeField] private Load _load;
    [SerializeField] private Save _save;
    [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
    [SerializeField] private ExtensionMap _extensionMap;

    private List<ItemPosition> _currentItemPositions = new List<ItemPosition>();
    private List<Territory> _currentTerritories = new List<Territory>();
    private List<FinderPositions> _currentFinderPositions = new List<FinderPositions>();
    private List<Territory> _extensionFilterTerritories = new List<Territory>();
    private Transform _container;
    private Map _currentMap;
    private int _index;
    private bool _initExtension;

    public int Index => _index;

    public int AmountMaps;

    public event Action IndexChanged;

    public Transform Container => _container;

    public List<FinderPositions> FinderPositions => _currentFinderPositions;

    public List<Territory> Territories => _currentTerritories;

    public List<ItemPosition> ItemPositions => _currentItemPositions;

    public Map CurrentMap => _currentMap;

    public Transform[] Environments => _environments;

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
        if (_environments[_index].GetComponent<Map>().IsMapExpanding)
        {
            ExtensionFillLists();
            return;
        }

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

    public void ExtensionFillLists()
    {
        _currentItemPositions = new List<ItemPosition>();
        _currentTerritories = new List<Territory>();
        _currentFinderPositions = new List<FinderPositions>();
        _extensionFilterTerritories = new List<Territory>();

        _container = _environments[_index];
        _currentMap = _environments[_index].GetComponent<Map>();

        Territory[] territories = _environments[_index].GetComponentsInChildren<Territory>(true);

        foreach (var territory in territories)
        {
            if (territory.IsExpanding)
            {
                _extensionFilterTerritories.Add(territory);
            }
        }

        int amount = _load.Get(ExtensionTerritory + _currentMap.Index, 0);

        Debug.Log("ExtensionTerritory " + ExtensionTerritory + _currentMap.Index);
        /*Debug.Log("AMOUNT " + amount);
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

            if (!_currentTerritories.Contains(territory) && !territory.IsExpanding ||
                !_currentTerritories.Contains(territory) && territory.IsOpened)
            {
                _currentTerritories.Add(territory);
            }
        }

        foreach (Territory territory in _currentTerritories)
        {
            FinderPositions[] finderPositionsInTerritory =
                territory.gameObject.GetComponentsInChildren<FinderPositions>(true);


            foreach (FinderPositions finder in finderPositionsInTerritory)
            {
                if (!_currentFinderPositions.Contains(finder))
                {
                    _currentFinderPositions.Add(finder);
                }
            }
        }

        foreach (Territory territory in _currentTerritories)
        {
            ItemPosition[] itemPositions = territory.gameObject.GetComponentsInChildren<ItemPosition>(true);


            foreach (ItemPosition itemPosition in itemPositions)
            {
                if (!itemPosition.enabled)
                    continue;

                if (!_currentItemPositions.Contains(itemPosition))
                {
                    _currentItemPositions.Add(itemPosition);
                }
            }
        }

        if (_environments[_index].GetComponent<Map>().IsMapExpanding)
        {
            _extensionMap.SetMap(_environments[_index].GetComponent<Map>());
            _extensionMap.ContinueMap(_currentTerritories,
                _currentItemPositions, _currentFinderPositions, _extensionFilterTerritories);
        }

        if (_initExtension)
        {
            _extensionMap.ResetMap(_environments[_index].GetComponent<Map>(), _currentTerritories,
                _currentItemPositions, _currentFinderPositions, _extensionFilterTerritories);
            _initExtension = false;
        }
    }

    public void SetPositions(List<ItemPosition> positions)
    {
        _currentItemPositions = positions;

        _visualItemsDeactivator.SetPositions(_currentItemPositions);
    }

    public void ResetTerritory()
    {
        _currentItemPositions = new List<ItemPosition>();

        Territory[] territories = _environments[_index].GetComponentsInChildren<Territory>(true);

        foreach (var territory in territories)
        {
            if (territory.IsExpanding)
            {
                territory.SetClose();
                territory.gameObject.SetActive(false);
            }
        }

        _save.SetData(ExtensionTerritory + _environments[_index].GetComponent<Map>().Index, 0);

        if (_environments[_index].GetComponent<Map>().IsWaterRandom)
            _extensionMap.RandomWater(_environments[_index].GetComponent<Map>());

        // _save.SetData(ExtensionTerritory, 0);
        
        _initExtension = true;
        ExtensionFillLists();
    }
}