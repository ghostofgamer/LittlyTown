using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectionMovement : MonoBehaviour
{
    private float[] _positions;
    private bool _runIt = false;
    private float _time;
    private Button _takeButton;
    private int _buttonNumber;
    private float _scrollPosition = 0;
    private float _factor = 2f;
    public Scrollbar _scrollbar;
    private float _speedlerp = 1f;
    private float _progress = 0.1f;
    private float _targetDistance = 0f;
    private float _localPositionZ = 0f;
    private Coroutine _coroutine;
    private int _currentIndex;

    public event Action<int> PositionScrolled; 
    
    
    
    private float _step = 46;
    private int _index;
    private float _elapsedTime = 0f;
    private float _duration = 1f;
    private Vector3 _target;
    private Vector3 _currentPosition;
    private Vector3 _startPosition;
    [SerializeField]private Vector3 _startResetPosition;
    private float _currentStep;
    private float _currentZ;
    
    
    
    
    
    
    
    
    
    
    
    /*void Start()
    {
        _positions = new float[transform.childCount];
        // ActivationDescription(0);
        float distance = 1f / (_positions.Length - 1f);

        for (int i = 0; i < _positions.Length; i++)
        {
            _positions[i] = distance * i;
        }
    }

    public void ChangeValue(int value)
    {
        _currentIndex += value;
        
        if (_currentIndex >= _positions.Length)
            _currentIndex = 0;

        if (_currentIndex < 0)
            _currentIndex = _positions.Length - 1;
Debug.Log("сгккуте " + _currentIndex);
        _buttonNumber = _currentIndex;
        // ActivationDescription(_currentIndex);
        PositionScrolled?.Invoke(_currentIndex);
        GecisiDuzenle(1f / (_positions.Length - 1f), _positions);
    }

    private void GecisiDuzenle(float distance, float[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (_scrollPosition < pos[i] + (distance / _factor) && _scrollPosition > pos[i] - (distance / _factor))
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(Scrolling(pos));
            }
        }
    }

    private IEnumerator Scrolling(float[] pos)
    {
        float elapsedTime = 0;
        float duration = 0.15f;
        float startvalue = _scrollbar.value;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _scrollbar.value =
                Mathf.Lerp(startvalue, pos[_buttonNumber], elapsedTime / duration);
            yield return null;
        }

        _scrollbar.value = pos[_buttonNumber];
    }*/
}