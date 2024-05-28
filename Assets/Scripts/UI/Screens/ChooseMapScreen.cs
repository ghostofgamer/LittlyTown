using Dragger;
using SaveAndLoad;
using UI.Screens;
using UnityEngine;

public class ChooseMapScreen : AbstractScreen
{
    private const string LastActiveMap = "LastActiveMap";

    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private Load _load;
    [SerializeField]private CameraMovement _cameraMovement;

    private int _startValue = 0;
    private int _currentvalue;
    
    
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
    }

    public override void Close()
    {
        base.Close();
        _cameraMovement.ResetZoom();
    }

    private void CheckActivation()
    {
        _currentvalue = _load.Get(LastActiveMap, _startValue);

        if (_currentvalue > _startValue)
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