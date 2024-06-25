using System;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

namespace SandBoxContent
{
    public class Cleaner : MonoBehaviour
    {
        private Item _item;
        private int _layerMask;
        private int _layer = 3;
        private ItemPosition _lastItemPosition;
        private ItemPosition _currentItemPosition;
        private RaycastHit _hit;
        private Ray _ray;

        public event Action ItemRemoved;

        private void Start()
        {
            _layerMask = 1 << _layer;
            _layerMask = ~_layerMask;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask))
                {
                    if (_hit.transform.TryGetComponent(out ItemPosition itemPosition))
                    {
                        if (_currentItemPosition != itemPosition)
                            itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();

                        _currentItemPosition = itemPosition;
                        _lastItemPosition = itemPosition;

                        if (itemPosition.IsBusy)
                        {
                            itemPosition.Item.gameObject.SetActive(false);
                            itemPosition.ClearingPosition();
                            ItemRemoved?.Invoke();
                            return;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_lastItemPosition != null)
                    _lastItemPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
            }
        }
    }
}