using UnityEngine;

namespace PlatformInitContent
{
    [CreateAssetMenu(menuName = "Build/Platform Config")]
    public class PlatformConfig : ScriptableObject
    {
        public bool GooglePlay;
        public bool RuStore;
        public bool YandexWeb;
    }
}