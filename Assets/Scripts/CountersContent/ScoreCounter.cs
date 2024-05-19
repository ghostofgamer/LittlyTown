using System;
using ItemPositionContent;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CountersContent
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private Merger _merger;
        [SerializeField] private PositionMatcher _positionMatcher;
        [SerializeField] private TMP_Text _scoreTargetText;
        [SerializeField] private VisualScore _visualScore;
        [SerializeField] private DropGenerator _dropGenerator;
        [SerializeField] private LookMerger _lookMerger;

        private int _currentScore;
        private int _targetScore = 50;
        private int _scoreIncome = 0;
        private Vector3 _targetPosition;
        private string _scoreText= "гол {0} очков";
        
        public event Action<int, int> ScoreChanged;

        public event Action LevelChanged;
        
        public int CurrentScore => _currentScore;
        
        private void OnEnable()
        {
            _merger.Merged += AddIncome;
            _positionMatcher.NotMerged += AddGoal;
            _lookMerger.NotMerged += AddGoal;
        }

        private void OnDisable()
        {
            _merger.Merged -= AddIncome;
            _positionMatcher.NotMerged -= AddGoal;
            _lookMerger.NotMerged -= AddGoal;
        }

        private void Start()
        {
            Show();
        }

        public void SetValue(int value)
        {
            _currentScore = value;
            ScoreChanged?.Invoke(_currentScore, _targetScore);
        }
        
        private void AddIncome(int countMatch, int reward, ItemPosition itemPosition)
        {
            _scoreIncome += reward * countMatch;
            _targetPosition = itemPosition.transform.position;

            // _visualScore.transform.position = itemPosition.transform.position;
        }

        private void AddGoal()
        {
            if (_scoreIncome <= 0)
                return;

            _currentScore += _scoreIncome;
            _visualScore.gameObject.SetActive(true);
            _visualScore.ScoreMove(_currentScore,_targetPosition);
            ScoreChanged?.Invoke(_currentScore, _targetScore);

            if (_currentScore >= _targetScore)
            {
                NextGoal();
                LevelChanged?.Invoke();
            }

            _scoreIncome = 0;
        }

        private void NextGoal()
        {
            _currentScore -= _targetScore;
            _targetScore *= 2;
            Show();
            _dropGenerator.NextLevel();
            ScoreChanged?.Invoke(_currentScore, _targetScore);
        }

        private void Show()
        {
            _scoreTargetText.text = string.Format(_scoreText, _targetScore);
        }
    }
}