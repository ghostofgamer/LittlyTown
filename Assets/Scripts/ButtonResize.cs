using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonResize : MonoBehaviour
{
    public Button _button;
    public TMP_Text _text;
    public float minWidth;

    private float previousWidth;

    void Start()
    {
        previousWidth = _text.preferredWidth;
    }

    void Update()
    {
        // Получаем текущий размер текста
        float textWidth = _text.preferredWidth;

        // Проверяем, изменился ли размер текста с предыдущего кадра
        if (previousWidth != textWidth)
        {
            // Проверяем, превышает ли текущий размер текста минимальный размер
            if (textWidth > minWidth)
            {
                // Устанавливаем новый размер кнопки, основываясь на текущем размере текста
                RectTransform buttonRectTransform = _button.GetComponent<RectTransform>();
                buttonRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textWidth);
                LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRectTransform);
            }
            else
            {
                // Устанавливаем минимальный размер кнопки
                RectTransform buttonRectTransform = _button.GetComponent<RectTransform>();
                buttonRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
                LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRectTransform);
            }

            // Обновляем предыдущий размер текста
            previousWidth = textWidth;
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    /*
    [SerializeField ] private Button _button;
    [SerializeField ] private TMP_Text _text;

    private string _previousText;

    private void Start()
    {
        _previousText = _text.text;
        _text.text = _text.text;
    }

    private void Update()
    {
        if (_previousText != _text.text)
        {
            _previousText = _text.text;
            ValueChangeCheck();
        }
    }

    private void ValueChangeCheck()
    {
        float textWidth = _text.rectTransform.sizeDelta.x;

        RectTransform buttonRectTransform = _button.GetComponent<RectTransform>();
        buttonRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textWidth);
        LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRectTransform);
    }*/
}
