using System.Collections;
using System.Collections.Generic;
using ItemPositionContent;
using UnityEngine;

namespace ItemContent
{
    public class LightHouseTrigger : MonoBehaviour
    {
        private float _radius = 4f;
        private List<House> _houses = new List<House>();
        private List<House> _housesToRemove = new List<House>();
        private Collider[] _colliders;
        private float _factor = 2f;
        private Vector3 _center;
        private Vector3 _size;

        public void LookAround()
        {
            _center = transform.position;
            _size = Vector3.one * _radius * _factor;
            _colliders = Physics.OverlapBox(_center, _size / _factor, Quaternion.identity);

            foreach (Collider collider in _colliders)
            {
                if (collider.TryGetComponent(out ItemPosition itemPosition) && itemPosition.IsBusy &&
                    itemPosition.Item.IsHouse)
                {
                    House house = itemPosition.Item.GetComponent<House>();

                    if (!_houses.Contains(house))
                    {
                        _houses.Add(house);
                        house.IncreaseIncome();
                    }
                }
            }

            foreach (House house in _houses)
            {
                if (!((IList) _colliders).Contains(house.GetComponent<Collider>()))
                {
                    _housesToRemove.Add(house);
                    house.ResetIncome();
                }
            }

            foreach (House house in _housesToRemove)
                _houses.Remove(house);
        }

        public void RemoveHouses()
        {
            foreach (var house in _houses)
                house.ResetIncome();

            _houses.Clear();
            _housesToRemove.Clear();
        }
    }
}