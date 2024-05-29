using UnityEngine;

namespace ADS
{
    public abstract class AD : MonoBehaviour
    {
        public abstract void Show();
   
        private int _activeValue = 1;
        private int _inactiveValue = 0;
   
        protected virtual void OnOpen()
        {
            SetValue(_inactiveValue);
        }
   
        protected virtual void OnClose(bool isClosed)
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
