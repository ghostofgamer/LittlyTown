using Agava.YandexGames;

namespace ADS
{
    public class FullAds : AD
    {
        public override void Show()
        {
            if (YandexGamesSdk.IsInitialized)
                InterstitialAd.Show(OnOpen, OnClose);
        }
    }
}