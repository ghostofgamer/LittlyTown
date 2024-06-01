namespace SaveAndLoad.GoalContent
{
    [System.Serializable]
    public class GoalData
    {
        public int Index;
        public int CurrentValue;
        public int CompleteValue;

        public GoalData(int index, int currentValue, int completeValue)
        {
            Index = index;
            CurrentValue = currentValue;
            CompleteValue = completeValue;
        }
    }
}