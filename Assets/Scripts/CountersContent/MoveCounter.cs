using System;
using Dragger;
using SaveAndLoad;
using TMPro;
using UI.Buttons.UpgradeButtons;
using UnityEngine;

namespace CountersContent
{
    public class MoveCounter : MonoBehaviour
    {
        private const string StepCount = "StepCount";

        [SerializeField] private float _moveCount;
        [SerializeField] private TMP_Text _moveCountText;
        [SerializeField] private EndlessMoveButton _endlessMoveButton;
        [SerializeField] private GameObject _endless;
        [SerializeField] private GameObject _notEndless;
        [SerializeField] private Load _load;
        [SerializeField] private Save _save;
        [SerializeField] private ItemThrower _itemThrower;

        private float _maxValue = 300;
        private float _minValue = 0;
        private int _targetStepProfit = 5;
        private int _currentStep;
        private bool _isEndless;
        private float _startMoveCount = 100;

        public event Action MoveOver;

        public event Action StepProfitCompleted;

        public bool IsThereMoves => _moveCount > _minValue || _isEndless;

        public float MoveCount => _moveCount;

        private void OnEnable()
        {
            _itemThrower.PlaceChanged += OnCountChange;
            _endlessMoveButton.EndlessPurchased += OnSelectEndlessMoves;
        }

        private void OnDisable()
        {
            _itemThrower.PlaceChanged -= OnCountChange;
            _endlessMoveButton.EndlessPurchased -= OnSelectEndlessMoves;
        }

        private void Start()
        {
            _moveCount = _load.Get(StepCount, _startMoveCount);
            Show();
        }

        public void ReplenishSteps()
        {
            _moveCount = _maxValue;
            Show();
        }

        public void SetValue(float value)
        {
            _moveCount = value;
            Show();
            _save.SetData(StepCount, _moveCount);
        }

        private void OnCountChange()
        {
            if (_isEndless)
                return;

            _moveCount--;
            TakeStepsProfit();
            _moveCount = Mathf.Clamp(_moveCount, _minValue, _maxValue);
            _save.SetData(StepCount, _moveCount);

            if (_moveCount <= _minValue)
                MoveOver?.Invoke();

            Show();
        }

        private void Show()
        {
            _moveCountText.text = _moveCount.ToString();
        }

        private void TakeStepsProfit()
        {
            _currentStep++;

            if (_currentStep >= _targetStepProfit)
            {
                _currentStep = 0;
                StepProfitCompleted?.Invoke();
            }
        }

        private void OnSelectEndlessMoves()
        {
            _isEndless = true;
            _notEndless.SetActive(false);
            _endless.SetActive(true);
        }
    }
}