using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    private float _minFov = 30f;
    private float _maxFov = 90f;
    private float _mouseScrollSensitivity = 10f;
    private float _touchScrollSensitivity = 5f;
    private float _currentFov;

    private void Start()
    {
        enabled = false;
        _currentFov = _camera.fieldOfView;
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0)
        {
            _currentFov -= scroll * _mouseScrollSensitivity;
            _currentFov = Mathf.Clamp(_currentFov, _minFov, _maxFov);
            _camera.fieldOfView = _currentFov;
        }
        
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevMagnitude = Vector2.Distance(touch1PrevPos, touch2PrevPos);
            float currentMagnitude = Vector2.Distance(touch1.position, touch2.position);

            float deltaMagnitude = currentMagnitude - prevMagnitude;

            _currentFov -= deltaMagnitude * _touchScrollSensitivity;
            _currentFov = Mathf.Clamp(_currentFov, _minFov, _maxFov);
            _camera.fieldOfView = _currentFov;
        }
    }
}