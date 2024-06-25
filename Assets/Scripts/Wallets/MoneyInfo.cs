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
            _wallet.ValueChanged += OnShowPrice;
        }

        private void OnDisable()
        {
            _wallet.ValueChanged -= OnShowPrice;
        }

        private void OnShowPrice()
        {
            _priceText.text = _wallet.CurrentValue.ToString();
        }
    }
}