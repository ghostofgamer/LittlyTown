using System;
using System.Collections;
using UnityEngine;

public class ChooseMap : MonoBehaviour
{
    [SerializeField] private Transform _container;

    private float _step = 46;
    private int _currentIndex;
    private float _elapsedTime = 0f;
    private float _duration = 1f;
    private Vector3 _target;
    private Vector3 _currentPosition;
    private Vector3 _startPosition;
    [SerializeField]private Vector3 _startResetPosition;
    private float _currentStep;
    private float _currentZ;

    public event Action<int> MapChanged;

    private Vector3 _mouseDownPosition;
    private Vector3 _mouseCurrentPosition;
    private float _sensitivity = 1f;
    private float _mouseDelta;
    private Vector3 _startScrollPosition;
    
    [SerializeField]private bool _isWork = false;
    
    private void Start()
    {
        /*// Debug.Log(":::::");
        _startPosition = transform.position;
        _startResetPosition = _startPosition;
        _currentZ = _startPosition.z;
        Debug.Log("Start " + _startResetPosition);*/
    }

    private void Update()
    {
        if (!_isWork)
            return;
        
        
        if (Input.GetMouseButtonDown(0))
        {
            _mouseDownPosition = Input.mousePosition;
            _startScrollPosition = transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            _mouseCurrentPosition = Input.mousePosition;
            _mouseDelta = -(_mouseCurrentPosition.x - _mouseDownPosition.x);
            /*Debug.Log("DELTA " + _mouseDelta);
            Debug.Log("DELTA Mathf" + Mathf.Abs(_mouseDelta));*/

            if (Mathf.Abs(_mouseDelta) > 10f)
            {
                // float value = Mathf.InverseLerp(-50f, 50f, mouseDelta) * _sensitivity; // Преобразуем разницу в значение от -1 до 1
                float value = Mathf.Sign(_mouseDelta) * _sensitivity;
                // Debug.Log("Value " + value);
                _mouseDownPosition = _mouseCurrentPosition;
                Vector3 newPos = new Vector3(0, 0, value);
                transform.position += newPos * 30 * Time.deltaTime;
                // Debug.Log("Value " + value);

                /*int index = value > 0 ? 1 : -1; // Определяем индекс в зависимости от направления перемещения мыши

                mouseDownPosition = currentMousePosition; // Обновляем позицию мыши при нажатии*/
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Mathf.Abs(_mouseDelta) > 6)
            {
                if (_currentIndex + (int) Mathf.Sign(_mouseDelta) > 10 ||
                    _currentIndex + (int) Mathf.Sign(_mouseDelta) < 0)
                {
                    transform.position = _startScrollPosition;
                    return;
                }

                // Debug.Log("свайп сильный " + _mouseDelta);
                ChangeMap((int) Mathf.Sign(_mouseDelta));
                return;
            }

            transform.position = _startScrollPosition;
        }
    }

    public void ChangeMap(int index)
    {
        Debug.Log("ChangeMap " + index);
        _currentIndex += index;
        Debug.Log("Current  " + _currentIndex);
        
        if (_currentIndex > 10 || _currentIndex < 0)
        {
            _currentIndex = Mathf.Clamp(_currentIndex, 0, 10);
            return;
        }

        MapChanged?.Invoke(_currentIndex);
        _currentStep = _step * index;
        _currentZ += _currentStep;
        _target = new Vector3(_startPosition.x, _startPosition.y, _currentZ);
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

    public void SetPosition(int index)
    {
        Debug.Log("SetPosition " + index);
        
        _currentIndex = index; 
        _startPosition = transform.position;
        _currentZ = _startPosition.z;
        _currentStep = _step * index;
        // Debug.Log("Сложение " + _currentStep);
        _currentZ += _currentStep;
        // Debug.Log("ПОЛ " + _currentZ);
        _target = new Vector3(_startPosition.x, _startPosition.y, _currentZ);
        transform.position = _target;
        // _startPosition = 
    }
    
    public void ResetMapPosition()
    {
        _currentIndex = 0;
        MapChanged?.Invoke(_currentIndex);
        transform.position = _startResetPosition;
        _currentPosition = _startResetPosition;
        _currentZ = _currentPosition.z;
        // StartCoroutine(MapResetMove());
    }

    private IEnumerator MapResetMove()
    {
        _elapsedTime = 0;
        _currentPosition = transform.position;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_currentPosition, _startResetPosition, _elapsedTime / _duration);
            yield return null;
        }

        transform.position = _startResetPosition;
    }

    public void StartWork()
    {
        _isWork = true;
        StartCoroutine(StartUpdate());
    }

    public void StopWork()
    {
        _isWork = false;
    }

    private IEnumerator StartUpdate()
    {
        yield return new WaitForSeconds(1f);
    }
}
