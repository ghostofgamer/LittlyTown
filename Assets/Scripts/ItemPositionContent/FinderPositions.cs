using System;
using ItemPositionContent;
using UnityEngine;

public class FinderPositions : MonoBehaviour
{
    [SerializeField] private float _searchRadius;

    private ItemPosition _itemPosition;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _itemPosition = GetComponent<ItemPosition>();
    }

    private void Start()
    {
        // FindNeighbor();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _searchRadius);
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

                // Vector3 localTargetPosition = transform.InverseTransformPoint(itemPosition.transform.position);
                _targetPosition = itemPosition.transform.position;
                Debug.Log("Название " + itemPosition.name + " " + _targetPosition);

                if (_targetPosition.z > transform.position.z &&
                    Math.Abs(_targetPosition.x - transform.position.x) < 0.1f)
                {
                    northPosition = itemPosition;
                }

                if (_targetPosition.x < transform.position.x &&
                    Math.Abs(_targetPosition.z - transform.position.z) < 0.1f)
                {
                    westPosition = itemPosition;
                }

                if (_targetPosition.x > transform.position.x &&
                    Math.Abs(_targetPosition.z - transform.position.z) < 0.1f)
                {
                    eastPosition = itemPosition;
                }

                if (_targetPosition.z < transform.position.z &&
                    Math.Abs(_targetPosition.x - transform.position.x) < 0.1f)
                {
                    southPosition = itemPosition;
                }
            }

            _itemPosition.SetNeighbors(northPosition, westPosition, eastPosition, southPosition);
        }
    }
}