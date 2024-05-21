using SaveAndLoad;
using UI.Screens;
using UnityEngine;

public class FirstScreen : AbstractScreen
{
    private const string LastActiveMap = "LastActiveMap";

    [SerializeField] private Load _load;
    [SerializeField] private ContinueButton _continueButton;

    private int _startValue = 0;
    private int _currentValue;

    private void Start()
    {
        _currentValue = _load.Get(LastActiveMap, _startValue);
        _continueButton.gameObject.SetActive(_currentValue > 0);
    }
}