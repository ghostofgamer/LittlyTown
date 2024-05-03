using System;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

namespace Dragger
{
    public class ItemDragger : MonoBehaviour
    {
        private ItemPositionLooker _itemPositionLooker;
        private ItemPosition _startPosition;
        private Item _selectedObject;
        private Plane _objectPlane;
        private float _offset = 1.65f;
        private int _layerMaskIgnore;
        private int _layerMask;
        private int _layer = 3;
        private float _distance;
        
        public event Action PlaceChanged;

        public bool IsObjectSelected { get; private set; } = false;

        public Item SelectedObject => _selectedObject;

        public int LayerMask => _layerMask;

        private void Start()
        {
            _layerMask = 1 << _layer;
            _layerMask = ~_layerMask;
            _itemPositionLooker = GetComponent<ItemPositionLooker>();
            PlaceChanged?.Invoke();
        }

        public void SetItem(Item item, ItemPosition itemPosition)
        {
            _selectedObject = item;
            _startPosition = itemPosition;
            _startPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        }

        public void DragItem()
        {
            _itemPositionLooker.LookPosition();
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_objectPlane.Raycast(mouseRay, out _distance))
            {
                Vector3 mouseWorldPosition = mouseRay.GetPoint(_distance);
                mouseWorldPosition.y = _selectedObject.transform.position.y;
                _selectedObject.transform.position = mouseWorldPosition;
            }
        }

        public void SelectItem()
        {
            if (_selectedObject == null)
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.TryGetComponent(out Item item) && !item.IsActive)
                {
                    _objectPlane = new Plane(Vector3.up, _selectedObject.transform.position);
                    IsObjectSelected = true;
                }

                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                    {
                        Vector3 position = _selectedObject.transform.position;
                        position = new Vector3(position.x, position.y + _offset, position.z);
                        _selectedObject.transform.position = position;
                        _objectPlane = new Plane(Vector3.up, position);
                        IsObjectSelected = true;
                    }
                }
            }
        }

        public void ThrowItem()
        {
            if (_selectedObject == null)
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (!itemPosition.IsBusy)
                    {
                        _selectedObject.transform.position = hit.transform.position;
                        _selectedObject.Activation();
                        PlaceChanged?.Invoke();
                        itemPosition.DeliverObject(_selectedObject);
                        _selectedObject = null;
                        IsObjectSelected = false;
                    }
                    else
                    {
                        ReturnPosition();
                    }
                }

                else
                {
                    ReturnPosition();
                }
            }
        }

        private void ReturnPosition()
        {
            _selectedObject.transform.position = _startPosition.transform.position;
            IsObjectSelected = false;
            _startPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        }
    }
}