using System;
using ItemContent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoalContent
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private Item _currentItem;
        [SerializeField] private int _currentValue;
        [SerializeField] private int _maxValue;
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private GameObject _sliderInfo;
        [SerializeField] private GameObject _completeGoalButton;
        [SerializeField] private GameObject _progressInfo;
        [SerializeField] private GameObject _completeInfo;
        [SerializeField] private int _index;

        private int _reward;
        private int _scorePercentage;

        public event Action ValueChanged;

        public int CompleteValue { get; private set; }

        public int Index => _index;

        public int CurrentValue => _currentValue;

        public GameObject CompleteGoalButton => _completeGoalButton;

        private void Start()
        {
            Show();
        }

        public void StartGoal()
        {
            _currentValue = 0;
            _completeGoalButton.SetActive(false);
            _sliderInfo.SetActive(true);
            _progressInfo.SetActive(true);
            _completeInfo.SetActive(false);
            Show();
        }

        public void SetCompleteValue(int value)
        {
            CompleteValue = value;
            ValueChanged?.Invoke();
        }

        public void SetValue(int currentValue)
        {
            _currentValue = currentValue;
            Show();

            if (_currentValue >= _maxValue)
                FinishGoal();
        }

        protected void OnChangeValue(Item item)
        {
            if (_currentItem.ItemName != item.ItemName || _currentValue >= _maxValue)
                return;

            _currentValue++;
            Show();

            if (_currentValue >= _maxValue)
                FinishGoal();

            ValueChanged?.Invoke();
        }

        private void FinishGoal()
        {
            _completeGoalButton.SetActive(true);
            _sliderInfo.SetActive(false);
        }

        private void Show()
        {
            _scorePercentage = Mathf.RoundToInt(_currentValue / (float)_maxValue * 100f);
            _slider.value = _scorePercentage;
            _progressText.text = _currentValue + " / " + _maxValue;
        }
    }
}