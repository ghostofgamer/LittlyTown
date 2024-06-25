using CountersContent;
using Dragger;
using ItemContent;
using ItemPositionContent;
using Keeper;
using MergeContent;
using UI.Screens.PossibilitiesShopContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.PossibilitiesFiles
{
    public class AdditionalPossibilitiesButton : AbstractButton
    {
        [SerializeField] private OpenButton _openItemStoreButton;
        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _imageBackGroundButton;
        [SerializeField] private Sprite _whiteIcon;
        [SerializeField] private Sprite _blackIcon;
        [SerializeField] private Sprite _activatedImage;
        [SerializeField] private Sprite _notActivatedImage;
        [SerializeField] private AdditionalPossibilitiesButton _additionalPossibilitiesButton;
        [SerializeField] private PossibilitiesCounter _possibilitiesCounter;
        [SerializeField] private PossibilitiesShopScreen _possibilitiesShopScreen;
        [SerializeField] private LookMerger _lookMerger;

        private Item _temporaryItem;
        private ItemPosition _itemPosition;
        private bool _isActivated;

        protected override void OnClick()
        {
            _isActivated = !_isActivated;
            AudioSource.PlayOneShot(AudioSource.clip);

            if (_additionalPossibilitiesButton._isActivated)
                _additionalPossibilitiesButton.OnDeactivation();

            if (_possibilitiesCounter.PossibilitiesCount > 0)
            {
                if (_isActivated)
                {
                    _lookMerger.StopMoveMatch();
                    Activation();
                }
                else
                {
                    OnDeactivation();
                }
            }
            else
            {
                _possibilitiesShopScreen.OnOpen();
                _isActivated = false;
            }
        }

        protected virtual void Activation()
        {
            _openItemStoreButton.enabled = false;
            SetSprite(_blackIcon, _activatedImage);
            SaveTemporaryItem();
        }

        protected virtual void OnDeactivation()
        {
            if (_temporaryItem == null)
                return;

            _openItemStoreButton.enabled = true;
            _isActivated = false;
            ReturnItem();
            SetSprite(_whiteIcon, _notActivatedImage);
        }

        private void SetSprite(Sprite icon, Sprite imageBackGroundButton)
        {
            _imageBackGroundButton.sprite = imageBackGroundButton;
            _icon.sprite = icon;
        }

        private void SaveTemporaryItem()
        {
            _temporaryItem = _itemKeeper.SelectedObject;
            _itemKeeper.ClearSelectedItem();
            _itemPosition = _temporaryItem.ItemPosition;
            _itemPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
            _temporaryItem.gameObject.SetActive(false);
        }

        private void ReturnItem()
        {
            _itemKeeper.SetItem(_temporaryItem, _itemPosition);
            _itemKeeper.SelectedObject.gameObject.SetActive(true);
            _temporaryItem = null;
        }
    }
}