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
        FindNeighbor();
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
                _targetPosition = itemPosition.transform.position;

                if (_targetPosition.z > transform.position.z &&
                    Math.Abs(_targetPosition.x - transform.position.x) < 0.1f)
                {
                    northPosition = itemPosition;
                    Debug.Log(gameObject.name);
                    Debug.Log(" Сверху itemPosition " + itemPosition.name);
                }

                if (_targetPosition.x < transform.position.x &&
                    Math.Abs(_targetPosition.z - transform.position.z) < 0.1f)
                {
                    westPosition = itemPosition;
                    Debug.Log(gameObject.name);
                    Debug.Log(" Слева itemPosition " + itemPosition.name);
                }

                if (_targetPosition.x > transform.position.x &&
                    Math.Abs(_targetPosition.z - transform.position.z) < 0.1f)
                {
                    eastPosition = itemPosition;
                    Debug.Log(gameObject.name);
                    Debug.Log(" Справа itemPosition " + itemPosition.name);
                }

                if (_targetPosition.z < transform.position.z &&
                    Math.Abs(_targetPosition.x - transform.position.x) < 0.1f)
                {
                    southPosition = itemPosition;
                    Debug.Log(gameObject.name);
                    Debug.Log(" Снизу itemPosition " + itemPosition.name);
                }
            }

            _itemPosition.SetNeighbors(northPosition, westPosition, eastPosition, southPosition);
        }
    }
}