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
        public int CompleteValue { get; private set; }
        public event Action ValueChanged;
        
        public int Index => _index;
        
        public int CurrentValue => _currentValue;

        public GameObject CompleteGoalButton => _completeGoalButton;

        public int MaxValue => _maxValue;
        
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

        private void FinishGoal()
        {
            _completeGoalButton.SetActive(true);
            _sliderInfo.SetActive(false);
        }

        protected void ChangeValue(Item item)
        {
            if (_currentItem.ItemName != item.ItemName || _currentValue >= _maxValue)
                return;

            _currentValue++;
            Show();
            // Debug.Log("ChangeValueGoal ");

            if (_currentValue >= _maxValue)
            {
                FinishGoal();
            }

            ValueChanged?.Invoke();
        }

        public void SetCompleteValue(int value)
        {
            CompleteValue = value;
            ValueChanged?.Invoke();
        }

        public void SetValue(int currentValue)
        {
            // Debug.Log(currentValue);
            _currentValue = currentValue;
            Show();
            
            if (_currentValue >= _maxValue)
            {
                FinishGoal();
            }
        }
        
        private void Show()
        {
            _scorePercentage = Mathf.RoundToInt((float) _currentValue / (float) _maxValue * 100f);
            _slider.value = _scorePercentage;
            _progressText.text = _currentValue.ToString() + " / " + _maxValue.ToString();
        }
    }
}