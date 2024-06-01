using System.Collections.Generic;
using SaveAndLoad.GoalContent;
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

        public List<Goal> Goals => _goals;
        
        public List<Goal> CurrentGoals => _currentGoals;

        private void Start()
        {
            if (_goalSaver.TryLoadGoals())
            {
                foreach (var t in _goalSaver.Indexes)
                {
                    Debug.Log(t);
                    _currentGoals.Add(_goals[t]);
                    ActivationGoals();
                }
                
                _currentGoals[0].SetValue(_goalSaver.SaveGoalData.FirstGoal.CurrentValue);
                _currentGoals[1].SetValue(_goalSaver.SaveGoalData.SecondGoal.CurrentValue);


                if (_goalSaver.SaveGoalData.FirstGoal.CompleteValue > 0)
                {
                    _currentGoals[0].CompleteGoalButton.GetComponent<GoalCompleteButton>().CompleteGoal();
                }
                if (_goalSaver.SaveGoalData.SecondGoal.CompleteValue > 0)
                {
                    _currentGoals[1].CompleteGoalButton.GetComponent<GoalCompleteButton>().CompleteGoal();
                }
                /*Debug.Log("GoalSaver     true");
                foreach (var t in _goalSaver.Indexes)
                {
                    Debug.Log(t);
                    _currentGoals.Add(_goals[t]);
                    ActivationGoals();
                }

                foreach (Goal goal in _currentGoals)
                {
                    goal.SetValue();
                }*/
            }
            else
            {
                Debug.Log("GoalSaver     false");
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
            // Debug.Log("Добавялем рандом задачи");
            DeactivationGoals();
            _currentGoals.Clear();
            _tempGoals.Clear();

            foreach (var goal in _goals)
            {
                _tempGoals.Add(goal);
            }

            for (int i = 0; i < _maxGoalsCompleted; i++)
            {
                int randomIndex = Random.Range(0, _tempGoals.Count);
                _currentGoals.Add(_tempGoals[randomIndex]);
                _tempGoals.RemoveAt(randomIndex);
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