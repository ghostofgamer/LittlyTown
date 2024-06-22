using System.Collections.Generic;
using UI.Buttons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GoalContent
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GoalsCounter : MonoBehaviour
    {
        [SerializeField] private List<Goal> _goals = new List<Goal>();
        [SerializeField] private GoalSaver _goalSaver;

        private List<Goal> _currentGoals = new List<Goal>();
        private List<Goal> _tempGoals = new List<Goal>();
        private int _maxGoalsCompleted = 2;
        private int _currentGoalsCompleted = 0;
        private int _randomIndex;
        private int _firstGoalIndex = 0;
        private int _secondGoalIndex = 1;

        public List<Goal> Goals => _goals;

        public List<Goal> CurrentGoals => _currentGoals;

        private void Start()
        {
            if (_goalSaver.TryLoadGoals())
            {
                foreach (int index in _goalSaver.Indexes)
                {
                    _currentGoals.Add(_goals[index]);
                    ActivationGoals();
                }

                _currentGoals[_firstGoalIndex].SetValue(_goalSaver.SaveGoalData.FirstGoal.CurrentValue);
                _currentGoals[_secondGoalIndex].SetValue(_goalSaver.SaveGoalData.SecondGoal.CurrentValue);

                if (_goalSaver.SaveGoalData.FirstGoal.CompleteValue > 0)
                    _currentGoals[_firstGoalIndex].CompleteGoalButton.GetComponent<GoalCompleteButton>().CompleteGoal();

                if (_goalSaver.SaveGoalData.SecondGoal.CompleteValue > 0)
                    _currentGoals[_secondGoalIndex].CompleteGoalButton.GetComponent<GoalCompleteButton>().CompleteGoal();
            }
            else
            {
                if (_currentGoals.Count == 0)
                    InitializeGoals();
            }
        }

        public void CompleteGoal()
        {
            _currentGoalsCompleted++;

            if (_currentGoalsCompleted >= _maxGoalsCompleted)
            {
                _currentGoalsCompleted = 0;
                _goalSaver.UnSubscribe();
                InitializeGoals();
            }
        }

        private void InitializeGoals()
        {
            DeactivationGoals();
            _currentGoals.Clear();
            _tempGoals.Clear();

            foreach (var goal in _goals)
                _tempGoals.Add(goal);
            
            for (int i = 0; i < _maxGoalsCompleted; i++)
            {
                _randomIndex = Random.Range(0, _tempGoals.Count);
                _currentGoals.Add(_tempGoals[_randomIndex]);
                _tempGoals.RemoveAt(_randomIndex);
            }

            _goalSaver.SaveGoals(_currentGoals);
            ActivationGoals();
        }

        private void DeactivationGoals()
        {
            foreach (var goal in _goals)
                goal.gameObject.SetActive(false);
        }

        private void ActivationGoals()
        {
            foreach (var goal in _currentGoals)
            {
                goal.gameObject.SetActive(true);
                goal.StartGoal();
                goal.GetComponent<GoalAnimation>().ShowGoal();
            }
        }
    }
}