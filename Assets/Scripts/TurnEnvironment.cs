using System.Collections;
using System.Collections.Generic;
using ItemPositionContent;
using UnityEngine;

public class TurnEnvironment : MonoBehaviour
{
    [SerializeField] private GameObject[] _environments;
    [SerializeField] private GameObject _container;
    [SerializeField] private Initializator _initializator;
    [SerializeField]private LookMerger _lookMerger;
    
    private GameObject _currentEnvironment;
    
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
    private float _angle=0f ;
    private float _step = 90f;
    private bool _isRotating = false; 
    
    /*private void Update()
    {
        if (_isEndGameRotate)
            _environments[_initializator.Index].transform.Rotate(0, _speed * Time.deltaTime, 0);
        
        if (_isRotating)
        {
            UpdateRotation();
        }
    }*/
    
    private void Update()
    {
        /*if (_isEndGameRotate)
            _environments[_initializator.Index].transform.Rotate(0, _speed * Time.deltaTime, 0);*/
        
        if (_isRotating)
        {
            UpdateSandBoxRotation();
        }
    }
    
    private void UpdateRotation()
    {
        _environments[_initializator.Index].transform.rotation = Quaternion.Lerp(_environments[_initializator.Index].transform.rotation, Quaternion.Euler(_target), _speed * Time.deltaTime);

        if (_environments[_initializator.Index].transform.rotation == Quaternion.Euler(_target))
        {
            _isRotating = false;
        }
    }
    
    private void UpdateSandBoxRotation()
    {
        _currentEnvironment.transform.rotation = Quaternion.Lerp(_currentEnvironment.transform.rotation, Quaternion.Euler(_target), _speed * Time.deltaTime);

        if (_currentEnvironment.transform.rotation == Quaternion.Euler(_target))
        {
            _isRotating = false;
        }
    }
    
    public void ChangeRotation(int index)
    {
        _lookMerger.StopMoveMatch();
        _currentIndex += index;
        Debug.Log(_currentIndex);

        if (_currentIndex > _maxIndex)
            _currentIndex = _minIndex;

        if (_currentIndex < _minIndex)
            _currentIndex = _maxIndex;
        
        _angle += _step * index;
        
        _target = new Vector3(_environments[_initializator.Index].transform.rotation.x, _angle,
            _environments[_initializator.Index].transform.rotation.z);
        
        /*_target = new Vector3(_environments[_initializator.Index].transform.rotation.x, _angles[_currentIndex],
            _environments[_initializator.Index].transform.rotation.z);*/
        _isRotating = true;
        /*if (_coroutineRotate != null)
            StopCoroutine(_coroutineRotate);

        _coroutineRotate = StartCoroutine(Turn());*/
    }

    public void SetPositions(GameObject environment)
    {
        _currentEnvironment = environment;
    }
    
    public void ChangeSandBoxRotation(int index)
    {
        _lookMerger.StopMoveMatch();
        _currentIndex += index;
        Debug.Log(_currentIndex);

        /*if (_currentIndex > _maxIndex)
            _currentIndex = _minIndex;

        if (_currentIndex < _minIndex)
            _currentIndex = _maxIndex;*/
        
        _angle += _step * index;
        
        _target = new Vector3(_currentEnvironment.transform.rotation.x, _angle, _currentEnvironment.transform.rotation.z);
        
        /*_target = new Vector3(_environments[_initializator.Index].transform.rotation.x, _angles[_currentIndex],
            _environments[_initializator.Index].transform.rotation.z);*/
        _isRotating = true;
        /*if (_coroutineRotate != null)
            StopCoroutine(_coroutineRotate);

        _coroutineRotate = StartCoroutine(Turn());*/
    }

    private IEnumerator Turn()
    {
        _elapsedTime = 0;
        _startRotation = _environments[_initializator.Index].transform.rotation;

        while (_elapsedTime < _duration)
        {
            _environments[_initializator.Index].transform.rotation =
                Quaternion.Slerp(_startRotation, Quaternion.Euler(_target), 3*Time.deltaTime);
            _container.transform.rotation = _environments[_initializator.Index].transform.rotation;
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _environments[_initializator.Index].transform.rotation = Quaternion.Euler(_target);
        _container.transform.rotation = _environments[_initializator.Index].transform.rotation;
    }
    
    /*private IEnumerator Turn()
    {
        _elapsedTime = 0;
        _startRotation = _environments[_initializator.Index].transform.rotation;

        while (_elapsedTime < _duration)
        {
            _environments[_initializator.Index].transform.rotation =
                Quaternion.Slerp(_startRotation, Quaternion.Euler(_target), _elapsedTime / _duration);
            _container.transform.rotation = _environments[_initializator.Index].transform.rotation;
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _environments[_initializator.Index].transform.rotation = Quaternion.Euler(_target);
        _container.transform.rotation = _environments[_initializator.Index].transform.rotation;
    }
    */
    
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
        _angle = 0f;
        _elapsedTime = 0;
        _startRotation = _environments[_initializator.Index].transform.rotation;

        while (_elapsedTime < _durationReturn)
        {
            _environments[_initializator.Index].transform.rotation =
                Quaternion.Slerp(_startRotation, Quaternion.identity, _elapsedTime / _durationReturn);
            _container.transform.rotation = _environments[_initializator.Index].transform.rotation;
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _environments[_initializator.Index].transform.rotation = Quaternion.identity;
        _container.transform.rotation = _environments[_initializator.Index].transform.rotation;
    }
}
