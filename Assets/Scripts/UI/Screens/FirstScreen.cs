using System.Collections;
using InitializationContent;
using SaveAndLoad;
using UI.Screens;
using UnityEngine;

public class FirstScreen : AbstractScreen
{
    private const string LastActiveMap = "LastActiveMap";
    private const string ItemStorageSave = "ItemStorageSave";
    private const string ActiveMap = "ActiveMap";
    private const string Map = "Map";

    [SerializeField] private Load _load;
    [SerializeField] private ContinueButton _continueButton;
    [SerializeField] private Initializator _initializator;

    private int _startValue = 0;
    private int _currentValue;
    private int _currentValue1;

    private void Start()
    {
        int lastValue = _load.Get(LastActiveMap, _startValue);
        int currentMap = _load.Get("Map", _startValue);
        // Debug.Log("ПОСЛЕДНИЙ!!! " + LastValue);
        _currentValue = _load.Get(LastActiveMap, _startValue);
        _currentValue1 = _load.Get(ActiveMap + currentMap, _startValue);
        // _currentValue1 = _load.Get(Map, _startValue);
        // _save.SetData(Map, _initializator.Index);
        /*if (PlayerPrefs.HasKey(ItemStorageSave + _initializator.Index))
        {
            Debug.Log("Существует " + ItemStorageSave + _initializator.Index);
        }*/
        // Debug.Log("INDEX  = " + _initializator.Index);
        // Debug.Log("Current 0  = " + _currentValue);
        // Debug.Log("Current 1 = " + _currentValue1);
        bool value = _currentValue > 0 && _currentValue1 > 0;
        // Debug.Log("Value" + value);

        // _continueButton.gameObject.SetActive(_currentValue > 0);
        _continueButton.gameObject.SetActive(value);

        if (lastValue > 0)
            StartCoroutine(SmoothOpen());
        
            // StartCoroutine(SmoothOpen());
    }

    public override void Open()
    {
        base.Open();

        /*if (PlayerPrefs.HasKey(ItemStorageSave + _initializator.Index))
        {
            Debug.Log("Существует " + ItemStorageSave + _initializator.Index);
        }*/
        _currentValue = _load.Get(LastActiveMap, _startValue);
        _currentValue1 = _load.Get(ActiveMap + _initializator.Index, _startValue);
        /*Debug.Log("INDEX  = " + _initializator.Index);
        
        Debug.Log("Current 0  = " + _currentValue);
        Debug.Log("Current 1 = " + _currentValue1);*/
        bool value = _currentValue > 0 && _currentValue1 > 0;
        // Debug.Log("Value" + value);

        // _continueButton.gameObject.SetActive(_currentValue > 0);
        _continueButton.gameObject.SetActive(value);
    }

    private IEnumerator SmoothOpen()
    {
        yield return new WaitForSeconds(1.65f);
        Open();
    }
}