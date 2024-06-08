using System;
using SaveAndLoad;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Load _load;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private ItemsStorage _itemsStorage;
    [SerializeField] private Initializator _initializator;
    [SerializeField] private ChooseMap _chooseMap;

    private const string LastActiveMap = "LastActiveMap";
    private int _startValue = 0;

    private void Awake()
    {
    }

    private void Start()
    {
        int value = _load.Get(LastActiveMap, _startValue);

        // int currentMap = _load.Get("Map", _startValue);
        int currentMap = _load.Get("Map", _startValue);
        
        Debug.Log("!!!! " + currentMap);
        _chooseMap.SetPosition(currentMap);

        if (value == 0)
        {
            _initializator.SetIndex(value);
            _initializator.FillLists();

            // _mapGenerator.ShowFirstMap();
            _mapGenerator.ShowTestFirstMap(_initializator.Territories, _initializator.FinderPositions,
                _initializator.ItemPositions, _initializator.CurrentMap.RoadsContainer,_initializator.CurrentMap.StartItems);
            _mapGenerator.GenerationAllMap(1);
            _mapGenerator.GenerationAllMap(2);
            _mapGenerator.GenerationAllMap(3);
            _mapGenerator.GenerationAllMap(4);
            _mapGenerator.GenerationAllMap(5);
            _mapGenerator.GenerationAllMap(6);
            _mapGenerator.GenerationAllMap(7);
            _mapGenerator.GenerationAllMap(8);
            _mapGenerator.GenerationAllMap(9);
            _mapGenerator.GenerationAllMap(10);
            // Debug.Log("First");
        }
        else
        {
            _initializator.SetIndex(currentMap);
            _initializator.FillLists();

            // Debug.Log("Bootstrap " + currentMap);

            for (int i = 0; i < _initializator.AmountMaps; i++)
            {
                _mapGenerator.GenerationAllMap(i);
                
                
                /*if (i == currentMap)
                {
                    _mapGenerator.TestShowMap(_initializator.Territories, _initializator.FinderPositions,
                        _initializator.CurrentMap.RoadsContainer, _initializator.ItemPositions);
                    Debug.Log("Загружаем карту н7омер " + _initializator.CurrentMap.name + i);
                }
                else
                {
                    _mapGenerator.GenerationAllMap(i);
                    Debug.Log("Загружаем остальные  " + +i);
                }*/
            }

            /*Debug.Log(_initializator.CurrentMap.name);
            _mapGenerator.TestShowMap(_initializator.Territories, _initializator.FinderPositions,
                _initializator.CurrentMap.RoadsContainer, _initializator.ItemPositions);
            _mapGenerator.GenerationAllMap(1);
            _mapGenerator.GenerationAllMap(2);*/
            // _mapGenerator.ShowMap();
            // Agava.YandexGames.Utility.PlayerPrefs.Load(onSuccessCallback: _itemsStorage.Load);
            _itemsStorage.Load();
        }
    }

    /*private void Start()
    {
        int value = _load.Get(LastActiveMap, _startValue);

        if (value == 0)
        {
            _mapGenerator.ShowFirstMap();
        }
        else
        {
            _mapGenerator.ShowMap();
            // Agava.YandexGames.Utility.PlayerPrefs.Load(onSuccessCallback: _itemsStorage.Load);
            _itemsStorage.Load();
        }
    }*/
}