using System.Collections.Generic;
using GoalContent;
using JetBrains.Annotations;
using SaveAndLoad.GoalContent;
using UnityEngine;

public class GoalSaver : MonoBehaviour
{
    [SerializeField] private GoalsCounter _goalsCounter;

    private const string SaveGoal = "SaveGoal";

    private SaveGoalData _saveGoalData;
    private GoalData _firsGoalData;
    private GoalData _secondGoalData;
    private int[] _indexes;
    private Goal _firstGoal;
    private Goal _secondGoal;
    private List<Goal> _goalList = new List<Goal>();

    public int[] Indexes => _indexes;

    public SaveGoalData SaveGoalData => _saveGoalData;

    public void UnSubscribe()
    {
        Debug.Log("Отписка ");
        _firstGoal.ValueChanged -= SaveChanges;
        _secondGoal.ValueChanged -= SaveChanges;
    }

    public void SaveGoals(List<Goal> currentGoals)
    {
        Debug.Log("Сохранение нового списка целей");
        _goalList = currentGoals;
        _saveGoalData = new SaveGoalData
        {
            FirstGoal = new GoalData(currentGoals[0].Index, currentGoals[0].CurrentValue, currentGoals[0].CompleteValue),

            SecondGoal = new GoalData(currentGoals[1].Index, currentGoals[1].CurrentValue, currentGoals[1].CompleteValue)
        };

        _firstGoal = currentGoals[0];
        _secondGoal = currentGoals[1];
        _firstGoal.ValueChanged += SaveChanges;
        _secondGoal.ValueChanged += SaveChanges;

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
            _indexes = new int[2];
            _indexes[0] = _saveGoalData.FirstGoal.Index;
            _indexes[1] = _saveGoalData.SecondGoal.Index;

            _firstGoal = _goalsCounter.Goals[_saveGoalData.FirstGoal.Index];
            _secondGoal = _goalsCounter.Goals[_saveGoalData.SecondGoal.Index];
            _goalList.Clear();
            _goalList.Add(_goalsCounter.Goals[_saveGoalData.FirstGoal.Index]);
            _goalList.Add(_goalsCounter.Goals[_saveGoalData.SecondGoal.Index]);
            _firstGoal.ValueChanged += SaveChanges;
            _secondGoal.ValueChanged += SaveChanges;
            return true;
        }

        return false;
    }

    private void SaveChanges()
    {
        Debug.Log("Saving changes");
        _saveGoalData.FirstGoal.Index = _goalList[0].Index;
        _saveGoalData.FirstGoal.CurrentValue = _goalList[0].CurrentValue;
        _saveGoalData.FirstGoal.CompleteValue = _goalList[0].CompleteValue;
        _saveGoalData.SecondGoal.Index = _goalList[1].Index;
        _saveGoalData.SecondGoal.CurrentValue = _goalList[1].CurrentValue;
        _saveGoalData.SecondGoal.CompleteValue = _goalList[1].CompleteValue;

        string jsonData = JsonUtility.ToJson(_saveGoalData);
        PlayerPrefs.SetString(SaveGoal, jsonData);
        PlayerPrefs.Save();
    }
}