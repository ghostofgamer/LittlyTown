using System;
using Dragger;
using TMPro;
using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] private float _moveCount;
    [SerializeField] private TMP_Text _moveCountText;
    [SerializeField] private ItemDragger _itemDragger;

    private float _maxValue = 10;
    private float _minValue = 0;

    private int _targetStepProfit = 5;
    private int _currentStep;

    public event Action MoveOver;

    public event Action StepProfitMaded;

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

    public void ReplenishSteps()
    {
        _moveCount = _maxValue;
        Show();
    }
    
    private void OnCountChange()
    {
        _moveCount--;
        TakeStepsProfit();
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

    private void TakeStepsProfit()
    {
        _currentStep++;

        if (_currentStep >= _targetStepProfit)
        {
            _currentStep = 0;
            StepProfitMaded?.Invoke();
        }
    }
}