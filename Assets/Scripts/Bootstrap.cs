using System;
using SaveAndLoad;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Load _load;
    [SerializeField]private MapGenerator _mapGenerator;
    [SerializeField]private ItemsStorage _itemsStorage;

    private const string LastActiveMap = "LastActiveMap";
    private int _startValue = 0;

    private void Awake()
    {
      
    }

    private void Start()
    {
        int value = _load.Get(LastActiveMap, _startValue);

        if (value == 0)
        {
            _mapGenerator.ShowFirstMap();
        }
        else
        {
            _mapGenerator.ShowMap();
            _itemsStorage.Load();
        }
    }
}