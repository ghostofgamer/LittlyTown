using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GoalContent
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GoalsCounter : MonoBehaviour
    {
        [SerializeField] private List<Goal> _goals = new List<Goal>();

        private List<Goal> _currentGoals = new List<Goal>();
        private List<Goal> _tempGoals = new List<Goal>();
        private int _maxGoalsCompleted = 2;
        private int _currentGoalsCompleted = 0;

        public List<Goal> CurrentGoals => _currentGoals;
        
        private void Start()
        {
            if (_currentGoals.Count == 0)
                InitializeGoals();
        }

        public void CompleteGoal()
        {
            _currentGoalsCompleted++;

            if (_currentGoalsCompleted >= _maxGoalsCompleted)
            {
                _currentGoalsCompleted = 0;
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
                // Debug.Log("открыли");
                goal.gameObject.SetActive(true);
                goal.StartGoal();
                goal.GetComponent<GoalAnimation>().ShowGoal();
            }
        }
    }
}