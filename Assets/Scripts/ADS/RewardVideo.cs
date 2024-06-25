using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

namespace ADS
{
    public abstract class RewardVideo : Ad
    {
        [SerializeField] private Button _button;

        public override void Show()
        {
            if (YandexGamesSdk.IsInitialized)
                VideoAd.Show(OnOpen, OnReward, OnClose);
        }

        protected abstract void OnReward();

        protected override void OnClose()
        {
            base.OnClose();

            if (_button != null)
                _button.enabled = true;
        }
    }
}