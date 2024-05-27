using UI.Buttons;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RotationButton[] _rotationButtons;
    
    public void ActivationOrthographicMode()
    {
        _camera.orthographic = true;
    }
    public void DeactivationOrthographicMode()
    {
        _camera.orthographic = false;
    }
    
    
}
