using System.Collections;
using ItemPositionContent;
using UnityEngine;

namespace MapsContent
{
    public class PositionScaller : MonoBehaviour
    {
        [SerializeField] private Vector3 _scale;
        [SerializeField] private float _duration;
        [SerializeField] private Transform _tileTransform;
        [SerializeField] private ItemPosition _startTile;
        [SerializeField]  private ItemPosition _itemPosition;
        
        private float _elapsedTime;
        private float _offsetX = 1f;
        private float _offsetZ = 3f;
        private float _offsetY = 1.5f; 
        private Vector3 _startPosition;
        private Vector3 _target;

        public void ScaleChanged()
        {
            _itemPosition.SetFirstRoad(_startTile);
            StartCoroutine(Scaling());
        }

        private IEnumerator Scaling()
        {
            _target = transform.position;
            _startPosition = new Vector3(_target.x + _offsetX, _target.y + _offsetY, _target.z + _offsetZ);
            _elapsedTime = 0;

            while (_elapsedTime < _duration)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, _scale, _elapsedTime / _duration);
                _tileTransform.position = Vector3.Lerp(_startPosition, transform.position, _elapsedTime / _duration);
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = _scale;
            _tileTransform.position = transform.position;
        }
    }
}