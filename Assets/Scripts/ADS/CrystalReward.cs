using UnityEngine;
using Wallets;

namespace ADS
{
    public class CrystalReward : MonoBehaviour
    {
        [SerializeField] private int _crystalAmount;
        [SerializeField] private CrystalWallet _crystalWallet;

        public void AddCrystal()
        {
            _crystalWallet.IncreaseValue(_crystalAmount);
        }
    }
}
