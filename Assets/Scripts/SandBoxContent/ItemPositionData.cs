namespace SandBoxContent
{
    [System.Serializable]
    public class ItemPositionData
    {
        public bool IsWater;
        public bool IsBusy;
        public bool IsElevation;
        public bool IsTrail;
        public bool IsRoad;

        public ItemPositionData(bool isWater, bool isBusy, bool isElevation, bool isTrail, bool isRoad)
        {
            IsWater = isWater;
            IsBusy = isBusy;
            IsElevation = isElevation;
            IsTrail = isTrail;
            IsRoad = isRoad;
        }
    }
}
