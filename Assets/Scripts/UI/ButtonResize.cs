using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ButtonResize : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _minWidth;
        [SerializeField] private bool _isLeft;

        private float _previousWidth;
        private RectTransform _buttonRectTransform;

        private void Start()
        {
            _previousWidth = _text.preferredWidth;

            if (_text.preferredWidth > _minWidth)
                ChangeScale(_text.preferredWidth);
        }

        private void Update()
        {
            if (_previousWidth != _text.preferredWidth)
            {
                ChangeScale(_text.preferredWidth > _minWidth ? _text.preferredWidth : _minWidth);
                _previousWidth = _text.preferredWidth;
            }
        }

        private void ChangeScale(float target)
        {
            if (!_isLeft)
            {
                _buttonRectTransform = _button.GetComponent<RectTransform>();
                _buttonRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, target);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_buttonRectTransform);
            }
            else
            {
                _buttonRectTransform = _button.GetComponent<RectTransform>();
                float currentLeftPosition = _buttonRectTransform.anchoredPosition.x;
                float currentWidth = _buttonRectTransform.rect.width;
                _buttonRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, target);
                float newLeftPosition = currentLeftPosition - (target - currentWidth);
                _buttonRectTransform.anchoredPosition =
                    new Vector2(newLeftPosition, _buttonRectTransform.anchoredPosition.y);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_buttonRectTransform);
            }
        }
    }
}