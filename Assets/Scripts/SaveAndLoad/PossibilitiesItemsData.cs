using System.Collections;
using System.Collections.Generic;
using PossibilitiesContent;
using UnityEngine;

[System.Serializable]
public class PossibilitiesItemsData
{
    public PossibilitieBulldozer PossibilitieBulldozer;
    public int PriceBulldozer;
    public int PriceReplace;
    public PossibilitieReplace PossibilitieReplacer;

    public PossibilitiesItemsData(PossibilitieBulldozer bulldozer, PossibilitieReplace replacer, int buldozerPrice,
        int replacePrice)
    {
        PossibilitieBulldozer = bulldozer;
        PossibilitieReplacer = replacer;
        PriceBulldozer = buldozerPrice;
        PriceReplace = replacePrice;
    }
}