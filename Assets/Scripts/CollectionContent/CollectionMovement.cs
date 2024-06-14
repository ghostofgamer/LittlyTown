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

    [SerializeField] private GameObject[] _collectionItems;

    public event Action<int> PositionScrolled;

    private float _stepX = -13.63f;
    private float _stepZ = 13.63f;
    private int _index;
    private float _elapsedTime = 0f;
    private float _duration = 0.5f;
    private Vector3 _target;
    private Vector3 _currentPosition;
    private Vector3 _startPosition;
    [SerializeField] private Vector3 _startResetPosition;
    private float _currentStepX;
    private float _currentStepZ;
    private float _currentZ;
    private float _currentX;
    private float _startX;
    private float _startZ;
    private float _endX;
    private float _endZ;
    private int _maxIndex;

    private void Start()
    {
        _maxIndex = gameObject.transform.childCount - 1;
        // _maxIndex = _collectionItems.Length - 1;
        _startPosition = transform.position;
        _startResetPosition = _startPosition;
        _currentZ = _startPosition.z;
        _currentX = _startPosition.x;
        _startX = _startPosition.x;
        _startZ = _startPosition.z;
        _endX = _startX + _stepX * _maxIndex;
        _endZ = _startZ + _stepZ * _maxIndex;
        PositionScrolled?.Invoke(_currentIndex);
    }

    public void ChangeMap(int index)
    {
        Debug.Log("ChangeMap " + index);
        _currentIndex += index;
        Debug.Log("Current  " + _currentIndex);

        if (_currentIndex < 0)
        {
            _currentIndex = _maxIndex;
            PositionScrolled?.Invoke(_currentIndex);
            _currentZ = _endZ;
            _currentX = _endX;
            _target = new Vector3(_endX, _startPosition.y, _endZ);
            StartCoroutine(MapMove());
            return;
        }

        if (_currentIndex > _maxIndex)
        {
            _currentIndex = 0;
            PositionScrolled?.Invoke(_currentIndex);
            _currentZ = _startZ;
            _currentX = _startX;
            _target = new Vector3(_currentX, _startPosition.y, _currentZ);
            StartCoroutine(MapMove());
            return;
        }

        PositionScrolled?.Invoke(_currentIndex);
        // MapChanged?.Invoke(_currentIndex);
        _currentStepX = _stepX * index;
        _currentStepZ = _stepZ * index;
        _currentZ += _currentStepZ;
        _currentX += _currentStepX;
        _target = new Vector3(_currentX, _startPosition.y, _currentZ);
        StartCoroutine(MapMove());
    }

    private IEnumerator MapMove()
    {
        _elapsedTime = 0;
        _currentPosition = transform.position;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_currentPosition, _target, _elapsedTime / _duration);
            yield return null;
        }

        transform.position = _target;
    }


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