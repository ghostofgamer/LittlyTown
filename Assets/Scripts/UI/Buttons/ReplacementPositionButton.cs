using System;
using ItemPositionContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class ReplacementPositionButton : AbstractButton
    {
        [SerializeField] private PositionMatcher _positionMatcher;
        [SerializeField] private BulldozerButton _bulldozerButton;
        [SerializeField] private ReplacementPosition _replacementPosition;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _imageBackGroundButton;
        [SerializeField] private Sprite _whiteIcon;
        [SerializeField] private Sprite _blackIcon;
        [SerializeField] private Sprite _activatedImage;
        [SerializeField] private Sprite _notActivatedImage;

        private bool _isActivated = false;

        public event Action ReplaceActivated;
        public event Action ReplaceDeactivated;

        protected override void OnEnable()
        {
            base.OnEnable();
            _replacementPosition.PositionsChanged += OnClick;
            // _bulldozerButton.RemovalActivated += Deactivation;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _replacementPosition.PositionsChanged -= OnClick;
            // _bulldozerButton.RemovalActivated -= Deactivation;
        }

        protected override void OnClick()
        {
            _isActivated = !_isActivated;

            if (_isActivated)
            {
                ReplaceActivated?.Invoke();
                _positionMatcher.StopMoveMatch();
                SetSprite(_blackIcon, _activatedImage);
            }
            else
            {
                ReplaceDeactivated?.Invoke();
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
            _isActivated = false;
            SetSprite(_whiteIcon, _notActivatedImage);
        }
    }
}