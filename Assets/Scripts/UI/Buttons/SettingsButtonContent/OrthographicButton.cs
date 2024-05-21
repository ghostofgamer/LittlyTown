using UnityEngine;
using UnityEngine.UI;

public class OrthographicButton : SettingsChangeButton
{
    [SerializeField] private Image _toggleImage;

    private bool _isToggleOn;
    
    protected override void OnClick()
    {
        _isToggleOn = !_isToggleOn;

        _toggleImage.enabled = _isToggleOn;

        if (_isToggleOn)
        {
            Debug.Log("Act");
            Settings.ActivationOrthographicMode();
        }
        else
        {
            Debug.Log("Deact");
            Settings.DeactivationOrthographicMode();
        }
    }
}
