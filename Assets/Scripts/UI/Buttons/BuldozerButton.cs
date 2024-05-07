using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class BuldozerButton : AbstractButton
    {
        [SerializeField]private RemovalItems _removalItems;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _imageBackGroundButton;
        [SerializeField] private Sprite _whiteIcon;
        [SerializeField] private Sprite _blackIcon;
        [SerializeField] private Sprite _activatedImage;
        [SerializeField] private Sprite _notActivatedImage;

        private bool _isActivated = false;

        public event Action RemovalActivated;
        public event Action RemovalDeactivated;

        protected override void OnEnable()
        {
            base.OnEnable();
            _removalItems.ItemRemoved += OnClick;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _removalItems.ItemRemoved -= OnClick;
        }

        protected override void OnClick()
        {
            Debug.Log("ВЫКЛ");
            _isActivated = !_isActivated;

            if (_isActivated)
            {
                RemovalActivated?.Invoke();
                SetSprite(_blackIcon, _activatedImage);
            }
            else
            {
                Deactivation();
                RemovalDeactivated?.Invoke();
                // SetSprite(_whiteIcon, _notActivatedImage);
            }
        }

        private void SetSprite(Sprite icon, Sprite imageBackGroundButton)
        {
            _imageBackGroundButton.sprite = imageBackGroundButton;
            _icon.sprite = icon;
        }

        private void Deactivation()
        {
            Debug.Log("Тут");
            SetSprite(_whiteIcon, _notActivatedImage);
        }
    }
}