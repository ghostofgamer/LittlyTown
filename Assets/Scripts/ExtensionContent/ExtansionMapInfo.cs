using System;
using SaveAndLoad;
using TMPro;
using UnityEngine;

public class ExtansionMapInfo : MonoBehaviour
{
    private const string ExtensionTerritory = "ExtensionTerritory";
    [SerializeField] private ExtensionMap _extensionMap;
    [SerializeField] private string[] _informations;
    [SerializeField] private TMP_Text _parameterInfo;
    [SerializeField] private Load _load;
    [SerializeField] private int _index;

    private int _startValue = 0;
    private int _currentIndex;

    private void OnEnable()
    {
        // _extensionMap.IndexChanged += LoadIndex;
        LoadIndex();
    }

    private void OnDisable()
    {
        // _extensionMap.IndexChanged -= LoadIndex;
    }

    private void Start()
    {
        _currentIndex = _load.Get(ExtensionTerritory + _index, _startValue);
        Debug.Log(_currentIndex);
        SetParameter();
    }

    private void SetParameter()
    {
        _parameterInfo.text = _informations[_currentIndex];
    }

    private void LoadIndex()
    {
        _currentIndex = _load.Get(ExtensionTerritory + _index, _startValue);
        Debug.Log("какой индщекс то " + _currentIndex);
        SetParameter();
    }
}