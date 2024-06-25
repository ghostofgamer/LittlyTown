using System;
using InitializationContent;
using ItemPositionContent;
using MergeContent;
using SaveAndLoad;
using SpawnContent;
using TMPro;
using UnityEngine;

namespace CountersContent
{
    public class ScoreCounter : MonoBehaviour
    {
        private const string CurrentRecordScore = "CurrentRecordScore";

        [SerializeField] private Merger _merger;
        [SerializeField] private TMP_Text _scoreTargetText;
        [SerializeField] private TMP_Text _goalText;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private VisualScore _visualScore;
        [SerializeField] private DropGenerator _dropGenerator;
        [SerializeField] private LookMerger _lookMerger;
        [SerializeField] private Save _save;
        [SerializeField] private Initializator _initializator;

        private int _targetScore = 50;
        private int _stepScore = 50;
        private int _defaultTargetScore = 50;
        private int _scoreIncome;
        private Vector3 _targetPosition;
        private int _maxRecordScore;
        private int _minFactor = 1;

        public event Action<int, int> ScoreChanged;

        public event Action LevelChanged;

        public event Action<int> ScoreIncomeChanged;

        public int CurrentScoreRecord { get; private set; }

        public int Factor { get; private set; } = 1;

        public int CurrentScore { get; private set; }

        private void OnEnable()
        {
            _merger.Merged += OnAddIncome;
            _lookMerger.NotMerged += OnAddGoal;
        }

        private void OnDisable()
        {
            _merger.Merged -= OnAddIncome;
            _lookMerger.NotMerged -= OnAddGoal;
        }

        private void Start()
        {
            Show();
        }

        public void SetValue(int value, int factor)
        {
            CurrentScore = value;
            Factor = factor;
            _targetScore = Factor * _stepScore;
            Show();
            _dropGenerator.NextLevel(Factor);
            ScoreChanged?.Invoke(CurrentScore, _targetScore);
        }

        public void ResetScore()
        {
            CurrentScoreRecord = 0;
            _save.SetData(CurrentRecordScore + _initializator.Index, CurrentScoreRecord);
            CurrentScore = 0;
            _targetScore = _defaultTargetScore;
            Factor = _minFactor;
            _dropGenerator.ResetLevel();
            Show();
            ScoreChanged?.Invoke(CurrentScore, _targetScore);
        }

        public void SetCurrentScore(int currentValue)
        {
            CurrentScoreRecord = currentValue;
            _save.SetData(CurrentRecordScore + _initializator.Index, CurrentScoreRecord);
        }

        private void OnAddIncome(int countMatch, int reward, ItemPosition itemPosition)
        {
            _scoreIncome += reward * countMatch;
            CurrentScoreRecord += _scoreIncome;
            _save.SetData(CurrentRecordScore + _initializator.Index, CurrentScoreRecord);
        }

        private void OnAddGoal()
        {
            if (_scoreIncome <= 0)
                return;

            CurrentScore += _scoreIncome;
            _visualScore.gameObject.SetActive(true);
            _visualScore.ScoreMove(CurrentScore);
            ScoreChanged?.Invoke(CurrentScore, _targetScore);
            ScoreIncomeChanged?.Invoke(_scoreIncome);

            if (CurrentScore >= _targetScore)
            {
                NextGoal();
                LevelChanged?.Invoke();
            }

            _scoreIncome = 0;
        }

        private void NextGoal()
        {
            Factor++;
            CurrentScore -= _targetScore;
            _targetScore = Factor * _stepScore;
            Show();
            _dropGenerator.NextLevel(Factor);
            ScoreChanged?.Invoke(CurrentScore, _targetScore);
        }

        private void Show()
        {
            _scoreTargetText.text = _goalText.text + " " + _targetScore + " " + _score.text;
        }
    }
}