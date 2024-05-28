using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class SnapRotationChangerButton : SettingsChangeButton
{
    [SerializeField] private Image _toggleImage;
    [SerializeField] private RotationButton[] _rotationButtons;

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

    protected override void ChangeValue()
    {
        _toggleImage.enabled = IsToggleOn;

        if (IsToggleOn)
            ActivationButtons();
        else
            DeactivationButtons();

        SaveValue();
    }
}