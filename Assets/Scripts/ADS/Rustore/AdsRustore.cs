using System;
using AppMetricaContent;
using Io.AppMetrica;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class AdsRustore : MonoBehaviour
{
    public static AdsRustore Instance;

    private RewardedAdLoader rewardedAdLoader;
    private RewardedAd rewardedAd;
    private bool _isReady = false;

    private Action onReward;
    private Action onClose;
    private Action<string> onError;

    private const string AD_UNIT_ID = "R-M-18688729-2";
    // private const string AD_UNIT_ID = "demo-rewarded-yandex";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        MobileAds.SetAgeRestrictedUser(false);

        rewardedAdLoader = new RewardedAdLoader();
        rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
        rewardedAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;

        Load();
    }

    private void Load()
    {
        rewardedAdLoader.LoadAd(
            new AdRequestConfiguration.Builder(AD_UNIT_ID).Build()
        );
    }

    public bool GetStatus()
    {
        return _isReady;
    }

    public void Show(Action reward, Action close, Action<string> error)
    {
        if (rewardedAd == null)
        {
            error?.Invoke("Rewarded ad not loaded");
            Load();
            return;
        }

        onReward = reward;
        onClose = close;
        onError = error;

        rewardedAd.OnRewarded += HandleRewarded;
        rewardedAd.OnAdDismissed += HandleDismissed;
        rewardedAd.OnAdFailedToShow += HandleFailedToShow;

        rewardedAd.Show();
        
          AppMetrica.ReportEvent("ADS", AppMetricaActivator.ToJson(("RewardedAD", "ShowAttempt")));
    }

    #region Callbacks

    private void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
    {
        rewardedAd = args.RewardedAd;
        _isReady = true;
    }

    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        onError?.Invoke(args.Message);
    }

    private void HandleRewarded(object sender, Reward args)
    {
        onReward?.Invoke();
        
         AppMetrica.ReportEvent("ADS", AppMetricaActivator.ToJson(("RewardedAD", "Rewarded")));
    }

    private void HandleDismissed(object sender, EventArgs args)
    {
        rewardedAd.Destroy();
        rewardedAd = null;
        _isReady = false;

        onClose?.Invoke();
        AppMetrica.ReportEvent("ADS", AppMetricaActivator.ToJson(("RewardedAD", "Closed")));
        Load();
    }

    private void HandleFailedToShow(object sender, AdFailureEventArgs args)
    {
        onError?.Invoke(args.Message);
    }

    #endregion
}