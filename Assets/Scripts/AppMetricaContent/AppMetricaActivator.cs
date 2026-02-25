using System.Collections.Generic;
using UnityEngine;
using Io.AppMetrica;

namespace AppMetricaContent
{
    public class AppMetricaActivator : MonoBehaviour
    {
        private static readonly string _playerPrefsKey = "AppMetricaActivator-IsFirstLaunch";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ActivateAppMetrica()
        {
            AppMetricaConfig appMetricaConfig = new AppMetricaConfig("39342b38-5bc8-4c65-b28f-38be2f98652a")
            {
                CrashReporting = true,
                SessionTimeout = 10,
                LocationTracking = false,
                Logs = false,
                FirstActivationAsUpdate = !IsFirstLaunch(),
                DataSendingEnabled = true,
            };

            AppMetrica.Activate(appMetricaConfig);
        }

        private static bool IsFirstLaunch()
        {
            if (PlayerPrefs.HasKey(_playerPrefsKey))
            {
                return false;
            }

            PlayerPrefs.SetInt(_playerPrefsKey, 1);
            PlayerPrefs.Save();
            AppMetrica.ReportEvent("Init", ToJson(("IsFirstLaunch", "true")));
            return true;
        }

        public static string ToJson(params (string key, object value)[] pairs)
        {
            var entries = new List<string>();
            foreach (var pair in pairs)
            {
                string valueStr = pair.value == null ? "null" : $"\"{pair.value}\"";
                entries.Add($"\"{pair.key}\":{valueStr}");
            }

            return "{" + string.Join(",", entries) + "}";
        }
    }
}