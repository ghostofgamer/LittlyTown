using System;
using Dragger;
using TMPro;
using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] private float _moveCount;
    [SerializeField] private TMP_Text _moveCountText;
    [SerializeField] private ItemDragger _itemDragger;

    private float _maxValue = 300;
    private float _minValue = 0;

    public event Action MoveOver;

    public bool IsThereMoves => _moveCount > _minValue;

    private void Start()
    {
        Show();
    }

    private void OnEnable()
    {
        _itemDragger.PlaceChanged += OnCountChange;
    }

    private void OnDisable()
    {
        _itemDragger.PlaceChanged -= OnCountChange;
    }

    private void OnCountChange()
    {
        _moveCount--;
        _moveCount = Mathf.Clamp(_moveCount, _minValue, _maxValue);
        
        if (_moveCount <= _minValue)
        {
            MoveOver?.Invoke();
        }

        Show();
    }

    private void Show()
    {
        _moveCountText.text = _moveCount.ToString();
    }
}