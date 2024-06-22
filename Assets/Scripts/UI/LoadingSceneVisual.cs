using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadingSceneVisual : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private float _speed = 1;

        private void Update()
        {
            _image.transform.Rotate(0, 0, -_speed);
        }
    }
}