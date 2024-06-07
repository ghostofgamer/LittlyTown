using System.Collections;
using UnityEngine;

public class EnvironmentMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] _environments;
    [SerializeField] private GameObject _container;
    [SerializeField] private Initializator _initializator;
    [SerializeField]private MapActivator _mapActivator;
    
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _elapsedTime;
    private float _duration = 1f;
    private Coroutine _coroutine;
    private Coroutine _coroutineRotate;

    private bool _isActive;

    private void Start()
    {
        _startPosition = _environments[_initializator.Index].transform.position;
        _targetPosition = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z + 500);
    }

    public void GoAway()
    {
        _startPosition = _environments[_initializator.Index].transform.position;
        _targetPosition = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z + 500);
        // _mapActivator.ChangeActivityMaps();
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _isActive = false;
        _coroutine = StartCoroutine(Move(_startPosition, _targetPosition));
    }

    public void ReturnPosition()
    {
        _environments[_initializator.Index].SetActive(true);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _isActive = true;
        _coroutine = StartCoroutine(Move(_targetPosition, _startPosition));
    }

    private IEnumerator Move(Vector3 startPosition, Vector3 targetPosition)
    {
        _elapsedTime = 0f;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            _environments[_initializator.Index].transform.position = Vector3.Lerp(startPosition, targetPosition, _elapsedTime / _duration);
            // _container.transform.position = _environments[_initializator.Index].transform.position;
            yield return null;
        }

        _environments[_initializator.Index].transform.position = targetPosition;
        _environments[_initializator.Index].SetActive(_isActive);
        
        /*if(_isActive)
            _mapActivator.ActivateAllMaps();*/
    }
}