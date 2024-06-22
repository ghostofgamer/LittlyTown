using PossibilitiesContent;

namespace SaveAndLoad
{
    [System.Serializable]
    public class PossibilitiesItemsData
    {
        public PossibilitieBulldozer PossibilitieBulldozer;
        public int PriceBulldozer;
        public int PriceReplace;
        public PossibilitieReplace PossibilitieReplacer;

        public PossibilitiesItemsData(PossibilitieBulldozer bulldozer, PossibilitieReplace replacer, int bulldozerPrice,
            int replacePrice)
        {
            PossibilitieBulldozer = bulldozer;
            PossibilitieReplacer = replacer;
            PriceBulldozer = bulldozerPrice;
            PriceReplace = replacePrice;
        }
    }
}