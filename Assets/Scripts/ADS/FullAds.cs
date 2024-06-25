using Agava.YandexGames;

namespace ADS
{
    public class FullAds : Ad
    {
        public override void Show()
        {
            if (YandexGamesSdk.IsInitialized)
                InterstitialAd.Show(OnOpen, OnClose);
        }
    }
}