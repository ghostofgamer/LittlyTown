using PossibilitiesContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class BulldozerButton : AbstractButton
    {
        [SerializeField] private RemovalItems _removalItems;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _imageBackGroundButton;
        [SerializeField] private Sprite _whiteIcon;
        [SerializeField] private Sprite _blackIcon;
        [SerializeField] private Sprite _activatedImage;
        [SerializeField] private Sprite _notActivatedImage;

        private bool _isActivated = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            _removalItems.Removed += OnClick;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _removalItems.Removed -= OnClick;
        }

        protected override void OnClick()
        {
            _isActivated = !_isActivated;

            if (_isActivated)
                SetSprite(_blackIcon, _activatedImage);
            else
                Deactivation();
        }

        private void SetSprite(Sprite icon, Sprite imageBackGroundButton)
        {
            _imageBackGroundButton.sprite = imageBackGroundButton;
            _icon.sprite = icon;
        }
        
        private void Deactivation()
        {
            _isActivated = false;
            SetSprite(_whiteIcon, _notActivatedImage);
        }
    }
}