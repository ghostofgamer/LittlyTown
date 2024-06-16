using Dragger;
using ItemContent;
using TMPro;
using UnityEngine;
using Wallets;

namespace UI.Buttons
{
    public class BuyItemButton : AbstractButton
    {
        [SerializeField] private Item _item;
        [SerializeField] private ItemDragger _itemDragger;
        [SerializeField] private Transform _container;
        [SerializeField] private GoldWallet _goldWallet;
        /*[SerializeField] private int _price;
        [SerializeField] private float _priceMultiplier;*/
        [SerializeField] private TMP_Text _priceText;
        [SerializeField]private CloseButton _closeButton;
        [SerializeField]private OpenButton _openButton;
        [SerializeField]private Initializator _initializator;

        private void Start()
        {
            Show();
        }

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            
            if (_goldWallet.CurrentValue < _item.Price)
                return;
            
            _goldWallet.DecreaseValue(_item.Price);
            Item item = Instantiate(_item, _initializator.CurrentMap.ItemsContainer);
            // _closeButton.Close();
            _openButton.Open();
            _itemDragger.SetTemporaryItem(item);
            _item.IncreasePrice();
            Show();
        }

        public void Show()
        {
            _priceText.text = _item.Price.ToString();
        }

        public void CheckPossibilityPurchasing()
        {
            _priceText.color = _item.Price > _goldWallet.CurrentValue ? Color.red : Color.white;
        }
    }
}