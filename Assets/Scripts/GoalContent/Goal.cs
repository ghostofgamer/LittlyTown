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
        [SerializeField] private GoalsScreen _goalsScreen;
        [SerializeField] private GameObject _progressInfo;
        [SerializeField] private GameObject _completeInfo;

        private int _reward;
        private int _scorePercentage;

        private void Start()
        {
            Show();
        }

        public void StartGoal()
        {
            _currentValue = 0;
            _completeInfo.SetActive(false);
            _progressInfo.SetActive(true);
            Show();
        }

        public void FinishGoal()
        {
            _completeInfo.SetActive(true);
            _progressInfo.SetActive(false);
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
                _goalsScreen.CompleteGoal();
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