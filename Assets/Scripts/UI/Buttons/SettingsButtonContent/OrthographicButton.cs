using UnityEngine;
using UnityEngine.UI;

public class OrthographicButton : SettingsChangeButton
{
    [SerializeField] private Image _toggleImage;
    [SerializeField] private Camera _camera; 

    protected override void ChangeValue()
    {
        _toggleImage.enabled = IsToggleOn;
        _camera.orthographic = IsToggleOn;
        SaveValue();
    }
}