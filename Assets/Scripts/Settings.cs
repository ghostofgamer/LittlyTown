using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    public void ActivationOrthographicMode()
    {
        _camera.orthographic = true;
    }
    public void DeactivationOrthographicMode()
    {
        _camera.orthographic = false;
    }
}
