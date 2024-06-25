using System;
using UnityEngine;

namespace ItemPositionContent
{
    [RequireComponent(typeof(ItemPosition))]
    public class FinderPositions : MonoBehaviour
    {
        private float _searchRadius = 1.6f;
        private ItemPosition _itemPosition;
        private Vector3 _targetPosition;
        private float _factor = 0.1f;

        private void Awake()
        {
            _itemPosition = GetComponent<ItemPosition>();
        }

        public void FindNeighbor()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _searchRadius);
            ItemPosition northPosition = null;
            ItemPosition westPosition = null;
            ItemPosition eastPosition = null;
            ItemPosition southPosition = null;

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (_itemPosition.IsElevation != itemPosition.IsElevation || itemPosition.IsWater)
                        continue;

                    _targetPosition = itemPosition.transform.position;

                    if (_targetPosition.z > transform.position.z &&
                        Math.Abs(_targetPosition.x - transform.position.x) < _factor)
                        northPosition = itemPosition;

                    if (_targetPosition.x < transform.position.x &&
                        Math.Abs(_targetPosition.z - transform.position.z) < _factor)
                        westPosition = itemPosition;

                    if (_targetPosition.x > transform.position.x &&
                        Math.Abs(_targetPosition.z - transform.position.z) < _factor)
                        eastPosition = itemPosition;

                    if (_targetPosition.z < transform.position.z &&
                        Math.Abs(_targetPosition.x - transform.position.x) < _factor)
                        southPosition = itemPosition;
                }

                _itemPosition.SetNeighbors(northPosition, westPosition, eastPosition, southPosition);
            }
        }
    }
}