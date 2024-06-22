using UnityEngine;

namespace UI
{
    public class ParticlesRotater : MonoBehaviour
    {
        [SerializeField] private float _direction;

        private float _speed = 30f;

        private void Update()
        {
            transform.Rotate(0, 0, _direction * _speed * Time.deltaTime);
        }
    }
}