using System;
using SaveAndLoad;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Load _load;
    [SerializeField]private MapGenerator _mapGenerator;

    private const string LastActiveMap = "LastActiveMap";
    private int _startValue = 0;

    private void Awake()
    {
        int value = _load.Get(LastActiveMap, _startValue);

        if (value == 0)
        {
            _mapGenerator.Generation();
            // _mapGenerator.ShowMap();
        }
    }

    private void Start()
    {
        int value = _load.Get(LastActiveMap, _startValue);

        /*if (value == 0)
        {
            // _mapGenerator.Generation();
            _mapGenerator.ShowMap();
        }*/

        if (value == 1)
        {
        }
    }
}