using UnityEngine;

namespace SDK
{
    public class GameReady : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
YandexGamesSdk.GameReady();
#endif
        }
    }
}