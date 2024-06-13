using System;
using ItemPositionContent;
using SaveAndLoad;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CountersContent
{
    public class ScoreCounter : MonoBehaviour
    {
        private const string CurrentRecordScore = "CurrentRecordScore";

        [SerializeField] private Merger _merger;
        [SerializeField] private PositionMatcher _positionMatcher;
        [SerializeField] private TMP_Text _scoreTargetText;
        [SerializeField] private TMP_Text _goalText;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private VisualScore _visualScore;
        [SerializeField] private DropGenerator _dropGenerator;
        [SerializeField] private LookMerger _lookMerger;
        [SerializeField] private Save _save;
        [SerializeField] private Initializator _initializator;

        private int _currentScore;
        private int _targetScore = 50;
        private int _stepScore = 50;
        private int _scoreIncome = 0;
        private Vector3 _targetPosition;
        private string _scoreText = "гол {0} очков";

        private int _maxRecordScore;
        public int CurrentScoreRecord { get; private set; }

        public event Action<int, int> ScoreChanged;

        public event Action LevelChanged;
        
        public event Action<int> ScoreIncomeChanged;

        public event Action<int> FactorChanged;
        
        public int Factor { get; private set; } = 1;

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

        public void SetValue(int value, int factor)
        {
            _currentScore = value;
            Factor = factor;
            FactorChanged?.Invoke(Factor);
            _targetScore = Factor * _stepScore;
            Show();
            _dropGenerator.NextLevel(Factor);
            ScoreChanged?.Invoke(_currentScore, _targetScore);
        }

        private void AddIncome(int countMatch, int reward, ItemPosition itemPosition)
        {
            _scoreIncome += reward * countMatch;
            _targetPosition = itemPosition.transform.position;
            CurrentScoreRecord += _scoreIncome;
            _save.SetData(CurrentRecordScore + _initializator.Index, CurrentScoreRecord);
            // Debug.Log("currentScore " + CurrentScoreRecord);
            // _visualScore.transform.position = itemPosition.transform.position;
        }

        private void AddGoal()
        {
            if (_scoreIncome <= 0)
                return;

            _currentScore += _scoreIncome;
            _visualScore.gameObject.SetActive(true);
            _visualScore.ScoreMove(_currentScore);
            ScoreChanged?.Invoke(_currentScore, _targetScore);
            ScoreIncomeChanged?.Invoke(_scoreIncome);
            
            if (_currentScore >= _targetScore)
            {
                NextGoal();
                LevelChanged?.Invoke();
            }

            _scoreIncome = 0;
        }

        private void NextGoal()
        {
            Factor++;
            _currentScore -= _targetScore;
            _targetScore = Factor * _stepScore;
            Show();
            _dropGenerator.NextLevel(Factor);
            ScoreChanged?.Invoke(_currentScore, _targetScore);
        }

        private void Show()
        {
            // _scoreTargetText.text = string.Format(_scoreText, _targetScore);
            _scoreTargetText.text = _goalText.text +" "+ _targetScore.ToString() +" "+ _score.text;
        }

        public void ResetScore()
        {
            CurrentScoreRecord = 0;
            _save.SetData(CurrentRecordScore + _initializator.Index, CurrentScoreRecord);
            _currentScore = 0;
            _targetScore = 50;
            Factor = 1;
            _dropGenerator.ResetLevel();
            Show();
            ScoreChanged?.Invoke(_currentScore, _targetScore);
        }

        public void SetCurrentScore(int currentValue)
        {
            CurrentScoreRecord = currentValue;
            _save.SetData(CurrentRecordScore + _initializator.Index, CurrentScoreRecord);
            // Debug.Log("Продолжить с рекордом " + CurrentScoreRecord);
        }
    }
}