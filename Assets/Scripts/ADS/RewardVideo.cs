using Agava.YandexGames;

namespace ADS
{
    public abstract class RewardVideo : AD
    {
        public override void Show()
        {
            if (YandexGamesSdk.IsInitialized)
                VideoAd.Show(OnOpen, OnReward, OnClose);
        }

        protected abstract void OnReward();
    }
}
