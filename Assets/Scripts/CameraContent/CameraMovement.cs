using System.Collections;
using UI.Buttons;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private OpenButton _openButton;

    private float _standartPerspectiveFOV = 30f;
    private float _perspectiveZoomUpValue = 70f;
    private float _perspectiveZoomDownValue = 50;

    private float _standartOrtographicSize = 6f;
    private float _ortographicSizeZoomIn = 13f;
    private float _ortographicSizeZoomOut = 10f;

    private float _elapsedTime;
    private float _duration = 1.5f;
    private float _currentFOVValue;
    private float _currentSizeValue;
    private Coroutine _coroutine;

    public void ZoomIn()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (!_camera.orthographic)
            _coroutine = StartCoroutine(StartChangeZoom(_perspectiveZoomUpValue));
        else
            _coroutine = StartCoroutine(StartChangeZoom(_ortographicSizeZoomIn));
    }

    public void ZoomOut()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (!_camera.orthographic)
            _coroutine = StartCoroutine(StartChangeZoom(_perspectiveZoomDownValue));
        else
            _coroutine = StartCoroutine(StartChangeZoom(_ortographicSizeZoomOut));
    }

    public void ResetZoom()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (!_camera.orthographic)
            _coroutine = StartCoroutine(StartChangeZoom(_standartPerspectiveFOV));
        else
            _coroutine = StartCoroutine(StartChangeZoom(_standartOrtographicSize));
    }


    private IEnumerator StartChangeZoom(float targetValue)
    {
        _openButton.enabled = false;
        _elapsedTime = 0;
        _currentFOVValue = _camera.fieldOfView;
        _currentSizeValue = _camera.orthographicSize;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;

            if (!_camera.orthographic)
                _camera.fieldOfView = Mathf.Lerp(_currentFOVValue, targetValue, _elapsedTime / _duration);
            else
                _camera.orthographicSize = Mathf.Lerp(_currentSizeValue, targetValue, _elapsedTime / _duration);

            yield return null;
        }
        
        _openButton.enabled = true;
    }
}