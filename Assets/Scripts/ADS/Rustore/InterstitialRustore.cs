using System;
using AppMetricaContent;
using Io.AppMetrica;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace ADS.Rustore
{
    public class InterstitialRustore : Ad
    {
        public static InterstitialRustore Instance;

        private InterstitialAdLoader interstitialAdLoader;
        private Interstitial interstitial;
        private bool _isReady = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            interstitialAdLoader = new InterstitialAdLoader();
            interstitialAdLoader.OnAdLoaded += HandleAdLoaded;
            interstitialAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;

            RequestAd();
        }

        private void RequestAd()
        {
            MobileAds.SetAgeRestrictedUser(false);
            const string AD_UNIT_ID = "R-M-18688729-1"; 
            // const string AD_UNIT_ID = "demo-interstitial-yandex"; 
            interstitialAdLoader.LoadAd(new AdRequestConfiguration.Builder(AD_UNIT_ID).Build());
            _isReady = false;
        }

        public bool GetStatus()
        {
            return _isReady;
        }

        public override void Show()
        {
            if (!_isReady || interstitial == null)
            {
                OnError("Interstitial ad is not ready yet");
                RequestAd(); 
                return;
            }
            
            OnOpen();
            
            interstitial.OnAdClicked += HandleAdClicked;
            interstitial.OnAdShown += HandleAdShown;
            interstitial.OnAdDismissed += HandleAdDismissed;
            interstitial.OnAdFailedToShow += HandleAdFailedToShow;
            interstitial.OnAdImpression += HandleImpression;

            interstitial.Show();
            AppMetrica.ReportEvent("ADS", AppMetricaActivator.ToJson(("InterstitialAD", "ShowAttempt")));
            _isReady = false;
        }

        #region Callbacks

        private void HandleAdLoaded(object sender, InterstitialAdLoadedEventArgs args)
        {
            interstitial = args.Interstitial;
            _isReady = true;
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            OnError(args.Message);
            _isReady = false;
        }

        private void HandleAdClicked(object sender, EventArgs args)
        {
            Debug.Log("Interstitial clicked");
        }

        private void HandleAdShown(object sender, EventArgs args)
        {
            Debug.Log("Interstitial shown");
            AppMetrica.ReportEvent("ADS", AppMetricaActivator.ToJson(("InterstitialAD", "Shown")));
        }

        private void HandleAdDismissed(object sender, EventArgs args)
        {
            OnClose(true);

            if (interstitial != null)
            {
                interstitial.Destroy();
                interstitial = null;
            }
            
            RequestAd();
        }

        private void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
        {
            OnError(args.Message);
        }

        private void HandleImpression(object sender, ImpressionData impressionData)
        {
            Debug.Log("Interstitial impression: " + (impressionData?.rawData ?? "null"));
        }

        #endregion
    }
}