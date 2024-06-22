using SandBoxContent;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : AbstractButton
{
    [SerializeField] private BuildButtonChanger _buildButtonChanger;
    [SerializeField] private int _index;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _notActiveSprite;
    [SerializeField] private Image _imageButton;

    private float _fullAlpha = 255f;
    private float _halfAlpha = 150f;

    public bool IsActive { get; private set; }

    protected override void OnClick()
    {
        AudioSource.PlayOneShot(AudioSource.clip);
        IsActive = !IsActive;
        
        if (IsActive)
            Activate();

        _buildButtonChanger.Deactivation(_index);
    }

    public void Activate()
    {
        IsActive = true;
        _imageButton.sprite = _activeSprite;
        // _imageButton.color = new Color(255, 255, 255, _halfAlpha);
    }
    
    public void Deactivate()
    {
        IsActive = false;
        _imageButton.sprite = _notActiveSprite;
        // _imageButton.color = new Color(255, 255, 255, _fullAlpha);
    }
}