using SandBoxContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.SandBoxButtons
{
    public class BuildButton : AbstractButton
    {
        [SerializeField] private BuildButtonChanger _buildButtonChanger;
        [SerializeField] private int _index;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _notActiveSprite;
        [SerializeField] private Image _imageButton;

        public bool IsActive { get; private set; }

        public void Activate()
        {
            IsActive = true;
            _imageButton.sprite = _activeSprite;
        }

        public void Deactivate()
        {
            IsActive = false;
            _imageButton.sprite = _notActiveSprite;
        }

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            IsActive = !IsActive;

            if (IsActive)
                Activate();

            _buildButtonChanger.Deactivation(_index);
        }
    }
}