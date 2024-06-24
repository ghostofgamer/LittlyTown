using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.SettingsButtonContent
{
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
}