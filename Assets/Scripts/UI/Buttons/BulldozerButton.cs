using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class BulldozerButton : AbstractButton
    {
        [SerializeField] private ReplacementPositionButton _replacementPositionButton;
        [SerializeField] private RemovalItems _removalItems;
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
            // _replacementPositionButton.ReplaceActivated += Deactivation;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _removalItems.ItemRemoved -= OnClick;
            // _replacementPositionButton.ReplaceActivated -= Deactivation;
        }

        protected override void OnClick()
        {
            _isActivated = !_isActivated;

            if (_isActivated)
            {
                RemovalActivated?.Invoke();
                SetSprite(_blackIcon, _activatedImage);
            }
            else
            {
                Deactivation();
            }
        }

        private void SetSprite(Sprite icon, Sprite imageBackGroundButton)
        {
            _imageBackGroundButton.sprite = imageBackGroundButton;
            _icon.sprite = icon;
        }
        
        private void Deactivation()
        {
            RemovalDeactivated?.Invoke();
            _isActivated = false;
            SetSprite(_whiteIcon, _notActivatedImage);
        }
    }
}