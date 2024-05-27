using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class SnapRotationChangerButton : AbstractButton
{
    [SerializeField] private Image _toggleImage;
    [SerializeField] private RotationButton[] _rotationButtons;

    private bool _isToggleOn;

    protected override void OnClick()
    {
        _isToggleOn = !_isToggleOn;

        _toggleImage.enabled = _isToggleOn;

        if (_isToggleOn)
            ActivationButtons();
        else
            DeactivationButtons();
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