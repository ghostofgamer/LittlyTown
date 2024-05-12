using ItemContent;
using TMPro;
using UnityEngine;
using Wallets;

namespace CountersContent
{
    public class GoldCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private Merger _merger;

        private int _profit;
        private int _stepCount;
        private int _currentStep;
        private string _scoreText = "{0} золота каждые 5 шагов";

        private void Awake()
        {
            Show();
        }

        private void OnEnable()
        {
            _moveCounter.StepProfitMaded += AddGold;
            _merger.ItemMergered += ChangeProfit;
        }

        private void OnDisable()
        {
            _moveCounter.StepProfitMaded -= AddGold;
        }

        private void AddGold()
        {
            if (_profit > 0)
                _goldWallet.AddValue(_profit);
        }

        private void ChangeProfit(Item item)
        {
            if (!item.IsHouse) return;

            _profit += item.Gold;
            Show();
        }

        private void Show()
        {
            _text.text = string.Format(_scoreText, _profit);
        }
    }
}