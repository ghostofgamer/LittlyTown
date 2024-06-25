using System.Collections;
using Dragger;
using InitializationContent;
using Keeper;
using MergeContent;
using PossibilitiesContent;
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
        [SerializeField] private LightHouseKeeper _lightHouseKeeper;
        [SerializeField] private RemovalItems _removalItems;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private ItemThrower _itemThrower;

        private int _profit;
        private int _stepCount;
        private int _currentStep;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.15f);

        private void Awake()
        {
            Show();
        }

        private void OnEnable()
        {
            _moveCounter.StepProfitCompleted += AddGold;
            _itemThrower.PlaceChanged += CheckIncome;
            _removalItems.Removed += CheckIncome;
            _merger.Mergered += CheckIncome;
            _lightHouseKeeper.CheckCompleted += CheckIncome;
        }

        private void OnDisable()
        {
            _moveCounter.StepProfitCompleted -= AddGold;
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
            _text.text = _profit + " " + _description.text;
        }

        public void CheckIncome()
        {
            StartCoroutine(StartSearchIncome());
        }

        private IEnumerator StartSearchIncome()
        {
            yield return _waitForSeconds;
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