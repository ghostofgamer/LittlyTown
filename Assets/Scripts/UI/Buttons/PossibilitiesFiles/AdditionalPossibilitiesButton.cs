using CountersContent;
using Dragger;
using ItemContent;
using ItemPositionContent;
using UI.Screens.PossibilitiesShopContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.BonusesContent
{
    public class AdditionalPossibilitiesButton : AbstractButton
    {
        [SerializeField] private OpenButton _openItemStoreButton;
        [SerializeField] private ItemDragger _itemDragger;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _imageBackGroundButton;
        [SerializeField] private Sprite _whiteIcon;
        [SerializeField] private Sprite _blackIcon;
        [SerializeField] private Sprite _activatedImage;
        [SerializeField] private Sprite _notActivatedImage;
        [SerializeField] private AdditionalPossibilitiesButton _additionalPossibilitiesButton;
        [SerializeField] private PossibilitiesCounter _possibilitiesCounter;
        [SerializeField] private PossibilitiesShopScreen _possibilitiesShopScreen;
        [SerializeField] private PositionMatcher _positionMatcher;
        [SerializeField] private LookMerger _lookMerger;

        private Item _temporaryItem;
        private ItemPosition _itemPosition;
        private bool _isActivated = false;

        public bool IsActivated => _isActivated;

        protected override void OnClick()
        {
            _isActivated = !_isActivated;
            AudioSource.PlayOneShot(AudioSource.clip);
            
            if (_additionalPossibilitiesButton.IsActivated)
                _additionalPossibilitiesButton.Deactivation();

            if (_possibilitiesCounter.PossibilitiesCount > 0)
            {
                if (_isActivated)
                {
                    _lookMerger.StopMoveMatch();
                    _positionMatcher.StopMoveMatch();
                    Activation();
                }
                else
                {
                    Deactivation();
                }
            }
            else
            {
                _possibilitiesShopScreen.Open();
                _isActivated = false;
            }
        }

        protected virtual void Activation()
        {
            _openItemStoreButton.enabled = false;
            SetSprite(_blackIcon, _activatedImage);
            SaveTemporaryItem();
        }

        private void SetSprite(Sprite icon, Sprite imageBackGroundButton)
        {
            _imageBackGroundButton.sprite = imageBackGroundButton;
            _icon.sprite = icon;
        }

        protected virtual void Deactivation()
        {
            if (_temporaryItem == null)
                return;
            
            _openItemStoreButton.enabled = true;
            _isActivated = false;
            ReturnItem();
            SetSprite(_whiteIcon, _notActivatedImage);
        }

        private void SaveTemporaryItem()
        {
            _temporaryItem = _itemDragger.SelectedObject;
            _itemDragger.ClearItem();
            _itemPosition = _temporaryItem.ItemPosition;
            _itemPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
            _temporaryItem.gameObject.SetActive(false);
        }

        private void ReturnItem()
        {
            _itemDragger.SetItem(_temporaryItem, _itemPosition);
            _itemDragger.SelectedObject.gameObject.SetActive(true);
            _temporaryItem = null;
        }
    }
}