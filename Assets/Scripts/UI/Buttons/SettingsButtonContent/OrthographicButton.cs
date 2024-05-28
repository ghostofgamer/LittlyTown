using UnityEngine;
using UnityEngine.UI;

public class OrthographicButton : SettingsChangeButton
{
    [SerializeField] private Image _toggleImage;

    protected override void ChangeValue()
    {
        _toggleImage.enabled = IsToggleOn;
        
        if (IsToggleOn)
        {
            Settings.ActivationOrthographicMode();
        }
        else
        {
            Settings.DeactivationOrthographicMode();
        }

        SaveValue();
    }
}