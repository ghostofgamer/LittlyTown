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
        [SerializeField] private TMP_Text _description;
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private Merger _merger;
        [SerializeField] private ItemDragger _itemDragger;
        [SerializeField] private LightHouseKeeper _lightHouseKeeper;
        [SerializeField] private RemovalItems _removalItems;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private ItemThrower _itemThrower;
        
        private int _profit;
        private int _stepCount;
        private int _currentStep;

        private void Awake()
        {
            Show();
        }

        private void OnEnable()
        {
            _moveCounter.StepProfitCompleted += AddGold;
            // _itemDragger.PlaceChanged += CheckIncome;
            _itemThrower.PlaceChanged += CheckIncome;
            _removalItems.Removed += CheckIncome;
            _merger.Mergered += CheckIncome;
            _lightHouseKeeper.CheckCompleted += CheckIncome;
        }

        private void OnDisable()
        {
            _moveCounter.StepProfitCompleted -= AddGold;
            // _itemDragger.PlaceChanged -= CheckIncome;
            _itemThrower.PlaceChanged -= CheckIncome;
            _removalItems.Removed -= CheckIncome;
            _merger.Mergered -= CheckIncome;
            _lightHouseKeeper.CheckCompleted -= CheckIncome;
        }

        private void AddGold()
        {
            if (_profit > 0)
                _goldWallet.IncreaseValue(_profit);
        }

        private void Show()
        {
            _text.text = _profit.ToString() + " " + _description.text;
        }

        public void CheckIncome()
        {
            StartCoroutine(StartSearchIncome());
        }

        private IEnumerator StartSearchIncome()
        {
            yield return new WaitForSeconds(0.15f);
            _profit = 0;

            foreach (var itemPosition in _initializator.ItemPositions)
            {
                if (itemPosition.IsBusy && itemPosition.Item.IsHouse)
                    _profit += itemPosition.Item.Gold;
            }

            Show();
        }
    }
}