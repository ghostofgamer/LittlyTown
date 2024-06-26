using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CountersContent
{
    [RequireComponent(typeof(ScoreCounter))]
    public class ScoreSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _percentText;

        private ScoreCounter _scoreCounter;
        private int _scorePercentage;
        private string _percent = "%";

        private void Awake()
        {
            _scoreCounter = GetComponent<ScoreCounter>();
            _percentText.text = _slider.value + _percent;
        }

        private void OnEnable()
        {
            _scoreCounter.ScoreChanged += OnAddValue;
        }

        private void OnDisable()
        {
            _scoreCounter.ScoreChanged -= OnAddValue;
        }

        private void OnAddValue(int currentScore, int targetScore)
        {
            _scorePercentage = Mathf.RoundToInt(currentScore / (float)targetScore * 100f);
            _slider.value = _scorePercentage;
            _percentText.text = _slider.value + _percent;
        }
    }
}