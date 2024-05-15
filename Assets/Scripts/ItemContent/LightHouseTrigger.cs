using System.Collections;
using System.Collections.Generic;
using ItemPositionContent;
using UnityEngine;

namespace ItemContent
{
    public class LightHouseTrigger : MonoBehaviour
    {
        public float radius = 10f;
        private List<House> _houses = new List<House>();
        private List<House> _housesToRemove = new List<House>();

        private Collider[] _colliders; 
        
        public void Look()
        {
            Vector3 center = transform.position;
            Vector3 size = Vector3.one * radius * 2f;

            _colliders = Physics.OverlapBox(center, size / 2f, Quaternion.identity);

            foreach (Collider collider in _colliders)
            {
                if (collider.TryGetComponent(out ItemPosition itemPosition) && itemPosition.IsBusy &&
                    itemPosition.Item.IsHouse)
                {
                    House house = itemPosition.Item.GetComponent<House>();
                    // Debug.Log("itemPosition.name " + itemPosition.name);
                    // Debug.Log("House.name " + house.name);

                    if (!_houses.Contains(house))
                    {
                        _houses.Add(house);
                        house.IncreaseIncome();
                        Debug.Log("itemPosition.name added " + house.name);
                    }
                }
            }

            foreach (House house in _houses)
            {
                if (!((IList) _colliders).Contains(house.GetComponent<Collider>()))
                {
                    _housesToRemove.Add(house);
                    house.ResetIncome();
                    Debug.Log("itemPosition.name removed " + house.name);
                }
            }

            foreach (House house in _housesToRemove)
            {
                _houses.Remove(house);
            }
        }

        /*private void Update()
        {
            Vector3 center = transform.position;
            Vector3 size = Vector3.one * radius * 2f;

            Collider[] colliders = Physics.OverlapBox(center, size / 2f, Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out ItemPosition itemPosition) && itemPosition.IsBusy &&
                    itemPosition.Item.IsHouse)
                {
                    House house = itemPosition.Item.GetComponent<House>();
                    // Debug.Log("itemPosition.name " + itemPosition.name);
                    // Debug.Log("House.name " + house.name);

                    if (!_houses.Contains(house))
                    {
                        _houses.Add(house);
                        house.IncreaseIncome();
                        Debug.Log("itemPosition.name added " + house.name);
                    }
                }
            }

            foreach (House house in _houses)
            {
                if (!((IList) colliders).Contains(house.GetComponent<Collider>()))
                {
                    _housesToRemove.Add(house);
                    house.ResetIncome();
                    Debug.Log("itemPosition.name removed " + house.name);
                }
            }

            foreach (House house in _housesToRemove)
            {
                _houses.Remove(house);
            }

            // Debug.Log("колличество домов " + _houses.Count);
        }*/

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector3 center = transform.position;
            Vector3 size = Vector3.one * radius * 2f;
            Gizmos.DrawWireCube(center, size);

            Vector3[] corners = new Vector3[4];
            corners[0] = center + new Vector3(-radius, 0f, -radius);
            corners[1] = center + new Vector3(radius, 0f, -radius);
            corners[2] = center + new Vector3(radius, 0f, radius);
            corners[3] = center + new Vector3(-radius, 0f, radius);

            for (int i = 0; i < 4; i++)
            {
                int nextIndex = (i + 1) % 4;
                Debug.DrawLine(corners[i], corners[nextIndex], Color.red);
            }
        }
    }
}