using System.Collections;
using System.Collections.Generic;
using ItemPositionContent;
using UI.Screens;
using UnityEngine;

public class SandBoxScreen : AbstractScreen
{
    [SerializeField] private List<ItemPosition> _itemPositions;
    [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
    [SerializeField] private CameraScrolling _cameraScrolling;
    [SerializeField] private TurnEnvironment _turnEnvironment;
    [SerializeField] private GameObject _environment;
    [SerializeField] private EnvironmentMovement _environmentMovement;
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private Vector3 _startPosition;
    
    private bool _isActive;
    private Coroutine _coroutine;
    private float _timeElapsed;
    private float _durationTime = 1f;
    private Vector3 _targetPosition;

    private void Start()
    {
        _visualItemsDeactivator.SetPositions(_itemPositions);
        _cameraScrolling.enabled = true;
    }

    public override void Open()
    {
        base.Open();
        _visualItemsDeactivator.SetPositions(_itemPositions);
        _cameraScrolling.enabled = true;
        _environmentMovement.GoAway();
        _turnEnvironment.SetPositions(_environment);
        ReturnPosition();
        _cameraMovement.ZoomOut();
    }

    public override void Close()
    {
        base.Close();
        _environmentMovement.ReturnPosition();
        GoAway();
        _cameraMovement.ResetZoom();
    }

    public void GoAway()
    {
        _targetPosition = new Vector3(_environment.transform.position.x, _environment.transform.position.y,
            _environment.transform.position.z + 500);
        // _mapActivator.ChangeActivityMaps();

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _isActive = false;
        _coroutine = StartCoroutine(Move(_environment.transform.position, _targetPosition));
    }

    public void ReturnPosition()
    {
        _environment.transform.position = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z - 500);
        _environment.SetActive(true);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _isActive = true;
        _coroutine = StartCoroutine(Move(_environment.transform.position, _startPosition));
    }

    private IEnumerator Move(Vector3 startPosition, Vector3 targetPosition)
    {
        _timeElapsed = 0f;

        while (_timeElapsed < _durationTime)
        {
            _timeElapsed += Time.deltaTime;
            _environment.transform.position = Vector3.Lerp(startPosition, targetPosition, _timeElapsed / _durationTime);
            // _container.transform.position = _environments[_initializator.Index].transform.position;
            yield return null;
        }

        _environment.transform.position = targetPosition;
        _environment.SetActive(_isActive);

        /*if(_isActive)
            _mapActivator.ActivateAllMaps();*/
    }
}