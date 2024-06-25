using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.SettingsButtonContent
{
    public class SnapRotationChangerButton : SettingsChangeButton
    {
        [SerializeField] private Image _toggleImage;
        [SerializeField] private RotationButton[] _rotationButtons;

        protected override void ChangeValue()
        {
            _toggleImage.enabled = IsToggleOn;

            if (IsToggleOn)
                ActivationButtons();
            else
                DeactivationButtons();

            SaveValue();
        }

        private void ActivationButtons()
        {
            foreach (var rotationButton in _rotationButtons)
                rotationButton.gameObject.SetActive(true);
        }

        private void DeactivationButtons()
        {
            foreach (var rotationButton in _rotationButtons)
                rotationButton.gameObject.SetActive(false);
        }
    }
}