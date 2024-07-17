using CountersContent;
using PostProcessContent;
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
        [SerializeField] private OpenButton _openButton;
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private bool _moveChecker;

        public void Open()
        {
            if (_moveChecker)
            {
                if (_moveCounter.MoveCount <= 0)
                {
                    _screenClose.Close();
                    return;
                }
            }

            if (_openButton != null)
                _openButton.gameObject.SetActive(true);

            if (_blur != null)
            {
                if (_isBluring)
                    _blur.TurnOn();
                else
                    _blur.TurnOff();
            }

            _screenClose.Close();

            if (_abstractScreen != null)
                _abstractScreen.OnOpen();
        }

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            Open();
        }
    }
}