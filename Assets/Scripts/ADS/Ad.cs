using UnityEngine;

namespace ADS
{
    public abstract class Ad : MonoBehaviour
    {
        private int _activeValue = 1;
        private int _inactiveValue = 0;

        public abstract void Show();

        protected void OnOpen()
        {
            SetValue(_inactiveValue);
        }

        protected void OnClose(bool isClosed)
        {
            SetValue(_activeValue);
        }

        protected virtual void OnClose()
        {
            SetValue(_activeValue);
        }

        private void SetValue(int value)
        {
            Time.timeScale = value;
            AudioListener.volume = value;
        }
    }
}