#if UNITY_EDITOR
using UnityEditor;

namespace PlatformInitContent
{
    public static class PlatformConfigApplier
    {
        [MenuItem("Build/Apply Platform Config")]
        public static void Apply()
        {
            var config = AssetDatabase.LoadAssetAtPath<PlatformConfig>(
                "Assets/Scripts/PlatformInitContent/PlatformConfig.asset");

            if (config == null)
            {
                UnityEngine.Debug.LogError("PlatformConfig not found");
                return;
            }

            // Проверка на ровно одну галочку
            int count = 0;
            if (config.GooglePlay) count++;
            if (config.RuStore) count++;
            if (config.YandexWeb) count++;

            if (count == 0)
            {
                UnityEngine.Debug.LogError("Please select **exactly one** platform in PlatformConfig!");
                return;
            }

            if (count > 1)
            {
                UnityEngine.Debug.LogError("Multiple platforms selected! Only one platform can be active at a time.");
                return;
            }

            // Применяем define
            string defines = "";
            if (config.GooglePlay) defines = "GOOGLE_PLAY";
            if (config.RuStore)    defines = "RUSTORE";
            if (config.YandexWeb)  defines = "YANDEX_PLATFORM";

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                BuildTargetGroup.Android, defines);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                BuildTargetGroup.WebGL, defines);

            UnityEngine.Debug.Log("Platform defines applied: " + defines);
        }
    }
}
#endif