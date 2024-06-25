using System.Collections;
using SandBoxContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.SandBoxButtons
{
    public class ChangeFunctionalityButton : AbstractButton
    {
        [SerializeField] private Sprite _activeButton;
        [SerializeField] private Sprite _notActiveButton;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private ChangeFunctionalityButton[] _changeFunctionalityButtons;
        [SerializeField] private RectTransform _content;
        [SerializeField] private Builder _builder;

        private float _positionCloseY = -143f;
        private float _positionOpenY = 143f;
        private float _elapsedTime;
        private float _duration = 0.16f;
        private bool _isActive;
        private Coroutine _coroutine;

        public void OffActivity()
        {
            _isActive = false;
        }

        public void Deactivation()
        {
            if (_builder != null)
                _builder.enabled = false;

            _buttonImage.sprite = _notActiveButton;
            _iconImage.color = Color.white;

            if (_content != null)
                CloseContent();
        }

        protected override void OnClick()
        {
            _isActive = !_isActive;

            if (_isActive)
            {
                DeactivationButtons();
                Activation();
            }
            else
            {
                Deactivation();
            }

            AudioSource.PlayOneShot(AudioSource.clip);
        }

        private void DeactivationButtons()
        {
            foreach (var changeFunctionalityButton in _changeFunctionalityButtons)
            {
                changeFunctionalityButton.Deactivation();
                changeFunctionalityButton.OffActivity();
            }
        }

        private void Activation()
        {
            if (_builder != null)
            {
                _builder.enabled = true;
                _builder.Open();
            }

            _buttonImage.sprite = _activeButton;
            _iconImage.color = Color.black;

            if (_content != null)
                OpenContent();
        }

        private void OpenContent()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            StartCoroutine(MoveContent(new Vector2(_content.anchoredPosition.x, _positionOpenY)));
        }

        private void CloseContent()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            StartCoroutine(MoveContent(new Vector2(_content.anchoredPosition.x, _positionCloseY)));
        }

        private IEnumerator MoveContent(Vector2 target)
        {
            _elapsedTime = 0;
            Vector2 startPosition = _content.anchoredPosition;

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                _content.anchoredPosition = Vector2.Lerp(startPosition, target, _elapsedTime / _duration);
                yield return null;
            }

            _content.anchoredPosition = target;
        }
    }
}