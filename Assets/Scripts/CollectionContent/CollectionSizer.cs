using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionSizer : MonoBehaviour
{
    [SerializeField] private float paddingLeftAndRight;

    private RectTransform _rectTransform;

    [SerializeField] private float spacing;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        var horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.spacing = spacing;

        float paddingLeft = (Screen.width) + (Screen.width * 0.5f);
        Debug.Log(paddingLeft);
        // SetContainerSizeAndPadding();
    }

    private void Update()
    {
        // float paddingLeft = (Screen.width) + (Screen.width * 0.5f);
        /*float screenWidth = Screen.width;

        Debug.Log(screenWidth);*/
        
        
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        // Debug.Log("Aspect ratio: " + aspectRatio);
    }


    // добавленный метод
    public void SetContainerSizeAndPadding()
    {
        float screenWidth = Screen.width;
        Debug.Log("screenWidth" + screenWidth);
        float containerWidth = screenWidth - (paddingLeftAndRight * 2);
        Debug.Log("containerWidth" + containerWidth * paddingLeftAndRight * 0.5f);

        /*float containerWidth = screenWidth - (paddingLeftAndRight * 2);

        _rectTransform.sizeDelta = new Vector2(containerWidth, _rectTransform.sizeDelta.y);

        HorizontalLayoutGroup layoutGroup = GetComponent<HorizontalLayoutGroup>();
        layoutGroup.padding.left = paddingLeftAndRight;
        layoutGroup.padding.right = paddingLeftAndRight;*/
    }

    // другие методы, которые вам нужны
}