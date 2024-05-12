using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWidthController : MonoBehaviour
{
    [SerializeField] private TMP_Text _textField;
    [SerializeField] private float _minWidth;

    /*private float _previousWidth;

    private void Start()
    {
        _previousWidth = _minWidth;
    }

    private void Update()
    {
        float textWidth = _textField.preferredWidth;

        if (_previousWidth != textWidth)
        {
            if (textWidth > _minWidth)
            {
                Debug.Log("выаываыва");
                Vector2 newSize = _textField.rectTransform.sizeDelta;
                newSize.x = textWidth;
                _textField.rectTransform.sizeDelta = newSize;
            }

            _previousWidth = textWidth;
        }
    }*/
}