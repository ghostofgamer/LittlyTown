using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEnvironment : MonoBehaviour
{
    [SerializeField] private GameObject _environment;
    [SerializeField] private GameObject _container;
    
    private Vector3 _target;
    private Quaternion _startRotation;
    private int _currentIndex;
    private int _maxIndex = 3;
    private int _minIndex = 0;
    private float[] _angles = new float[4] {0, -90, 180, 90};
    private Coroutine _coroutineRotate;
    private float _elapsedTime;
    private float _duration = 1f;
    private float _durationReturn = 0.15f;
    private bool _isEndGameRotate;
    private float _speed = 10;
    
    private void Update()
    {
        if (_isEndGameRotate)
            _environment.transform.Rotate(0, _speed * Time.deltaTime, 0);
    }
    
    public void ChangeRotation(int index)
    {
        _currentIndex += index;
        Debug.Log(_currentIndex);

        if (_currentIndex > _maxIndex)
            _currentIndex = _minIndex;

        if (_currentIndex < _minIndex)
            _currentIndex = _maxIndex;

        _target = new Vector3(_environment.transform.rotation.x, _angles[_currentIndex],
            _environment.transform.rotation.z);

        if (_coroutineRotate != null)
            StopCoroutine(_coroutineRotate);

        _coroutineRotate = StartCoroutine(Turn());
    }

    private IEnumerator Turn()
    {
        _elapsedTime = 0;
        _startRotation = _environment.transform.rotation;

        while (_elapsedTime < _duration)
        {
            _environment.transform.rotation =
                Quaternion.Slerp(_startRotation, Quaternion.Euler(_target), _elapsedTime / _duration);
            _container.transform.rotation = _environment.transform.rotation;
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _environment.transform.rotation = Quaternion.Euler(_target);
        _container.transform.rotation = _environment.transform.rotation;
    }
    
    public void StartRotate()
    {
        _isEndGameRotate = true;
    }

    public void StopRotate()
    {
        _isEndGameRotate = false;
        
        if (_coroutineRotate != null)
            StopCoroutine(_coroutineRotate);

        _coroutineRotate = StartCoroutine(ReturnRotate());
    }

    private IEnumerator ReturnRotate()
    {
        _elapsedTime = 0;
        _startRotation = _environment.transform.rotation;

        while (_elapsedTime < _durationReturn)
        {
            _environment.transform.rotation =
                Quaternion.Slerp(_startRotation, Quaternion.identity, _elapsedTime / _durationReturn);
            _container.transform.rotation = _environment.transform.rotation;
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _environment.transform.rotation = Quaternion.identity;
        _container.transform.rotation = _environment.transform.rotation;
    }
}
