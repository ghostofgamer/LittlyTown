using Dragger;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.BonusesContent
{
    public class AdditionalFeaturesButton : AbstractButton
    {
        [SerializeField] private OpenButton _openItemStoreButton;
        [SerializeField] private ItemDragger _itemDragger;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _imageBackGroundButton;
        [SerializeField] private Sprite _whiteIcon;
        [SerializeField] private Sprite _blackIcon;
        [SerializeField] private Sprite _activatedImage;
        [SerializeField] private Sprite _notActivatedImage;
        [SerializeField] private AdditionalFeaturesButton _additionalFeaturesButton;

        private Item _temporaryItem;
        private ItemPosition _itemPosition;
        private bool _isActivated = false;

        public bool IsActivated => _isActivated;

        protected override void OnClick()
        {
            _isActivated = !_isActivated;

            if (_additionalFeaturesButton.IsActivated)
                _additionalFeaturesButton.Deactivation();
            
            if (_isActivated)
            {
                Activation();
            }
            else
            {
                Deactivation();
            }
        }

        /*public virtual void Click()
        {
        }*/

        protected virtual void Activation()
        {
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