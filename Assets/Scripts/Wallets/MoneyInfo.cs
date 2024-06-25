using TMPro;
using UnityEngine;

namespace Wallets
{
    public class MoneyInfo : MonoBehaviour
    {
        [SerializeField] private AbstractWallet _wallet;
        [SerializeField] private TMP_Text _priceText;

        private void OnEnable()
        {
            _wallet.ValueChanged += ShowPrice;
        }

        private void OnDisable()
        {
            _wallet.ValueChanged -= ShowPrice;
        }

        private void ShowPrice()
        {
            _priceText.text = _wallet.CurrentValue.ToString();
        }
    }
}