using System.Collections;
using Dragger;
using ItemPositionContent;
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
        [SerializeField] private ItemPosition[] _itemPositions;
        [SerializeField] private ItemDragger _itemDragger;

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
            _itemDragger.PlaceChanged += CheckIncome;
            // _merger.ItemMergered += ChangeProfit;
            _merger.Mergered += CheckIncome;
        }

        private void OnDisable()
        {
            _moveCounter.StepProfitMaded -= AddGold;
            _itemDragger.PlaceChanged -= CheckIncome;
            // _merger.ItemMergered -= ChangeProfit;
            _merger.Mergered -= CheckIncome;
        }

        private void AddGold()
        {
            if (_profit > 0)
                _goldWallet.IncreaseValue(_profit);
        }

        /*private void ChangeProfit(Item item)
        {
            if (!item.IsHouse) return;

            _profit += item.Gold;
            Show();
        }*/

        private void Show()
        {
            _text.text = string.Format(_scoreText, _profit);
        }

        private void CheckIncome()
        {
            StartCoroutine(StartSearchIncome());
        }

        private IEnumerator StartSearchIncome()
        {
            yield return new WaitForSeconds(0.15f);

            _profit = 0;

            foreach (var itemPosition in _itemPositions)
            {
                if (itemPosition.IsBusy && itemPosition.Item.IsHouse)
                {
                    _profit += itemPosition.Item.Gold;
                }
            }

            Show();
        }
    }
}