using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class OpenButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _abstractScreen;
        [SerializeField] private AbstractScreen _screenClose;
        [SerializeField] private bool _isBluring;
        [SerializeField] private Blur _blur;

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            Open();
        }

        public void Open()
        {
            if (_blur != null)
            {
                if (_isBluring)
                {
                    _blur.TurnOn();
                }
                else
                {
                    _blur.TurnOff();
                }
            }

            _screenClose.Close();

            if (_abstractScreen != null)
                _abstractScreen.Open();
        }
    }
}