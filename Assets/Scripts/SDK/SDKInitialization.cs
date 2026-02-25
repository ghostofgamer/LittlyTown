using System.Collections;
#if UNITY_WEBGL && YANDEX_PLATFORM
using Agava.YandexGames;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDK
{
    public class SDKInitialization : MonoBehaviour
    {
        private const string MainScene = "MainScene";

        private void Awake()
        {
#if UNITY_WEBGL && YANDEX_PLATFORM
            YandexGamesSdk.CallbackLogging = true;
#endif
        }

        private IEnumerator Start()
        {
#if UNITY_WEBGL && YANDEX_PLATFORM
            yield return YandexGamesSdk.Initialize(OnInitialized);
            Debug.Log("Yandex SDK Initialized");

#elif UNITY_ANDROID && RUSTORE
            Debug.Log("Rustore SDK Initialized");
            SceneManager.LoadScene(MainScene);
            yield break;

#elif UNITY_ANDROID && GOOGLE_PLAY
            Debug.Log("Google SDK Initialized");
            SceneManager.LoadScene(MainScene);
            yield break;
#else
            // что угодно ещё
            SceneManager.LoadScene(MainScene);
            yield break;
#endif
        }

        private void OnInitialized()
        {
            SceneManager.LoadScene(MainScene);
        }
    }
}