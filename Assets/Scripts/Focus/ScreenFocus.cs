using Agava.WebUtility;
using UI.Screens;
using UnityEngine;

namespace Focus
{
    public class ScreenFocus : MonoBehaviour
    {
        [SerializeField] private SettingsScreen _settingsScreen;

        private int _stop = 0;
        private int _play = 1;

        private void OnEnable()
        {
            Application.focusChanged += OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
        }

        private void OnDisable()
        {
            Application.focusChanged -= OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
        }

        private void OnInBackgroundChangeApp(bool inApp)
        {
            SetValueAudio(!inApp);
            PauseGame(!inApp);
        }

        private void OnInBackgroundChangeWeb(bool isBackground)
        {
            SetValueAudio(isBackground);
            PauseGame(isBackground);
        }

        private void SetValueAudio(bool value)
        {
            AudioListener.pause = value;
        }

        private void PauseGame(bool value)
        {
            /*if (_settingsScreen != null && _settingsScreen.IsOpen)
                return;*/

            Time.timeScale = value ? _stop : _play;
        }
    }
}
