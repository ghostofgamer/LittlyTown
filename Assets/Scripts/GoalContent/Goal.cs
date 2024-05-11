using ItemContent;
using TMPro;
using UI.Screens;
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
        [SerializeField] private GoalsCounter _goalsCounter;
        [SerializeField] private GameObject _sliderInfo;
        [SerializeField] private GameObject _completeGoalButton;

        private int _reward;
        private int _scorePercentage;

        private void Start()
        {
            Show();
        }

        public void StartGoal()
        {
            _currentValue = 0;
            _completeGoalButton.SetActive(false);
            _sliderInfo.SetActive(true);
            Show();
        }

        public void FinishGoal()
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

            if (_currentValue >= _maxValue)
            {
                FinishGoal();
                // _goalsCounter.CompleteGoal();
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