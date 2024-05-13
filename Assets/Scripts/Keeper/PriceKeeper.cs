using PossibilitiesContent;
using UnityEngine;

namespace Keeper
{
    public class PriceKeeper : MonoBehaviour
    {
        [SerializeField] private PossibilitieBulldozer _possibilitiebulldozer;
        [SerializeField] private PossibilitieReplace _possibilitieReplace ;
        
        private int _priceBulldozer;
        private int _priceReplace;

        private void OnEnable()
        {
            _possibilitiebulldozer.PriceChanged += SavePriceBulldozer;
            _possibilitieReplace.PriceChanged += SavePriceReplace;
        }

        private void OnDisable()
        {
            _possibilitiebulldozer.PriceChanged -= SavePriceBulldozer;
            _possibilitieReplace.PriceChanged -= SavePriceReplace;
        }

        private void SavePriceBulldozer(int price)
        {
            _priceBulldozer = price;
            Debug.Log("увеличили цену бульдозеров " + _priceBulldozer);
        }
        
        private void SavePriceReplace(int price)
        {
            _priceReplace = price;
            Debug.Log("увеличили цену обмена " + _priceReplace);
        }
    }
}
