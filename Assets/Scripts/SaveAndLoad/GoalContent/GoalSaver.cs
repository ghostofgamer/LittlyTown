using System.Collections.Generic;
using GoalContent;
using UnityEngine;

namespace SaveAndLoad.GoalContent
{
    public class GoalSaver : MonoBehaviour
    {
        private const string SaveGoal = "SaveGoal";

        [SerializeField] private GoalsCounter _goalsCounter;

        private SaveGoalData _saveGoalData;
        private int[] _indexes;
        private Goal _firstGoal;
        private Goal _secondGoal;
        private List<Goal> _goalList = new List<Goal>();
        private int _firstGoalIndex = 0;
        private int _secondGoalIndex = 1;
        private int _maxGoals = 2;

        public int[] Indexes => _indexes;

        public SaveGoalData SaveGoalData => _saveGoalData;

        public void UnSubscribe()
        {
            _firstGoal.ValueChanged -= OnSaveChanges;
            _secondGoal.ValueChanged -= OnSaveChanges;
        }

        public void SaveGoals(List<Goal> currentGoals)
        {
            _goalList = currentGoals;

            _saveGoalData = new SaveGoalData
            {
                FirstGoal = new GoalData(
                    currentGoals[_firstGoalIndex].Index,
                    currentGoals[_firstGoalIndex].CurrentValue,
                    currentGoals[_firstGoalIndex].CompleteValue),

                SecondGoal = new GoalData(
                    currentGoals[_secondGoalIndex].Index,
                    currentGoals[_secondGoalIndex].CurrentValue,
                    currentGoals[_secondGoalIndex].CompleteValue),
            };

            _firstGoal = currentGoals[_firstGoalIndex];
            _secondGoal = currentGoals[_secondGoalIndex];
            _firstGoal.ValueChanged += OnSaveChanges;
            _secondGoal.ValueChanged += OnSaveChanges;
            string jsonData = JsonUtility.ToJson(_saveGoalData);
            PlayerPrefs.SetString(SaveGoal, jsonData);
            PlayerPrefs.Save();
        }

        public bool TryLoadGoals()
        {
            if (PlayerPrefs.HasKey(SaveGoal))
            {
                string jsonData = PlayerPrefs.GetString(SaveGoal);
                _saveGoalData = JsonUtility.FromJson<SaveGoalData>(jsonData);
                _indexes = new int[_maxGoals];
                _indexes[_firstGoalIndex] = _saveGoalData.FirstGoal.Index;
                _indexes[_secondGoalIndex] = _saveGoalData.SecondGoal.Index;
                _firstGoal = _goalsCounter.Goals[_saveGoalData.FirstGoal.Index];
                _secondGoal = _goalsCounter.Goals[_saveGoalData.SecondGoal.Index];
                _goalList.Clear();
                _goalList.Add(_goalsCounter.Goals[_saveGoalData.FirstGoal.Index]);
                _goalList.Add(_goalsCounter.Goals[_saveGoalData.SecondGoal.Index]);
                _firstGoal.ValueChanged += OnSaveChanges;
                _secondGoal.ValueChanged += OnSaveChanges;
                return true;
            }

            return false;
        }

        private void OnSaveChanges()
        {
            _saveGoalData.FirstGoal.Index = _goalList[_firstGoalIndex].Index;
            _saveGoalData.FirstGoal.CurrentValue = _goalList[_firstGoalIndex].CurrentValue;
            _saveGoalData.FirstGoal.CompleteValue = _goalList[_firstGoalIndex].CompleteValue;
            _saveGoalData.SecondGoal.Index = _goalList[_secondGoalIndex].Index;
            _saveGoalData.SecondGoal.CurrentValue = _goalList[_secondGoalIndex].CurrentValue;
            _saveGoalData.SecondGoal.CompleteValue = _goalList[_secondGoalIndex].CompleteValue;
            string jsonData = JsonUtility.ToJson(_saveGoalData);
            PlayerPrefs.SetString(SaveGoal, jsonData);
            PlayerPrefs.Save();
        }
    }
}