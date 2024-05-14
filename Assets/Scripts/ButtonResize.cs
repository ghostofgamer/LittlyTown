using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonResize : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _minWidth;
    [SerializeField] private bool _isLeft;

    private float _previousWidth;

    private void Start()
    {
        _previousWidth = _text.preferredWidth;

        if (_text.preferredWidth > _minWidth)
        {
            ChangeScale(_text.preferredWidth);
        }
    }

    private void Update()
    {
        if (_previousWidth != _text.preferredWidth)
        {
            if (_text.preferredWidth > _minWidth)
            {
                ChangeScale(_text.preferredWidth);
            }
            else
            {
                ChangeScale(_minWidth);
            }

            _previousWidth = _text.preferredWidth;
        }
    }

    private void ChangeScale(float target)
    {
        if (!_isLeft)
        {
            RectTransform buttonRectTransform = _button.GetComponent<RectTransform>();
            buttonRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, target);
            LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRectTransform);
            Debug.Log("DONTLeft");
        }
        else
        {
            Debug.Log("Left");

            RectTransform buttonRectTransform = _button.GetComponent<RectTransform>();
            
            // Сохраняем текущую позицию кнопки относительно левого края экрана
            float currentLeftPosition = buttonRectTransform.anchoredPosition.x;
            
            // Сохраняем текущую ширину кнопки
            float currentWidth = buttonRectTransform.rect.width;
            
            // Изменяем ширину кнопки
            buttonRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, target);
            
            // Вычисляем новую позицию кнопки относительно левого края экрана
            float newLeftPosition = currentLeftPosition - (target - currentWidth);
            
            // Устанавливаем новую позицию кнопки
            buttonRectTransform.anchoredPosition = new Vector2(newLeftPosition, buttonRectTransform.anchoredPosition.y);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRectTransform);
        }
    }
}