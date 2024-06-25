using UnityEngine;
using Wallets;

namespace ADS
{
    public class CrystalReward : RewardVideo
    {
        [SerializeField] private int _crystalAmount;
        [SerializeField] private CrystalWallet _crystalWallet;

        protected override void OnReward()
        {
            _crystalWallet.IncreaseValue(_crystalAmount);
        }
    }
}