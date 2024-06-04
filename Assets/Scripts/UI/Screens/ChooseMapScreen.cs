using Dragger;
using SaveAndLoad;
using UI.Screens;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChooseMapScreen : AbstractScreen
{
    private const string LastActiveMap = "LastActiveMap";

    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private Load _load;
    [SerializeField]private CameraMovement _cameraMovement;
    [SerializeField] private ChooseMap _chooseMap;
    [SerializeField] private GameObject _mapInformation;
    [SerializeField] private GameObject[] _mapInformations;
    [SerializeField] private Initializator _initializator;
    private int _startValue = 0;
    private int _currentValue;
    
    private void Start()
    {
        CheckActivation();
    }

    public override void Open()
    {
        CheckActivation();
        // _inputItemDragger.enabled = false;
        base.Open();
        _itemDragger.SwitchOff();
        _cameraMovement.ZoomOut();
        _chooseMap.enabled = true;
        _mapInformation.SetActive(true);
        _mapInformations[_initializator.Index].SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        _cameraMovement.ResetZoom();
        _chooseMap.enabled = false;
        _mapInformation.SetActive(false);
    }

    private void CheckActivation()
    {
        _currentValue = _load.Get(LastActiveMap, _startValue);

        if (_currentValue > _startValue)
        {
            _startButton.SetActive(false);
            _continueButton.SetActive(true);
            _restartButton.SetActive(true);
        }
        else
        {
            _startButton.SetActive(true);
            _continueButton.SetActive(false);
            _restartButton.SetActive(false);
        }
    }
}