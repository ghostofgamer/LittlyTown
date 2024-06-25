namespace SaveAndLoad
{
    [System.Serializable]
    public class PossibilitiesItemsData
    {
        public int PriceBulldozer;
        public int PriceReplace;

        public PossibilitiesItemsData(
            int bulldozerPrice,
            int replacePrice)
        {
            PriceBulldozer = bulldozerPrice;
            PriceReplace = replacePrice;
        }
    }
}