using System;
using System.Collections.Generic;
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
        private float _offset = 3f;
        private int _layerMaskIgnore;
        private int _layerMask;
        private int _layer = 3;
        private float _distance;
        private Item _temporaryItem;
        private bool _istemporary;

        public event Action PlaceChanged;
        public event Action<Item> BuildItem;

        public bool IsObjectSelected { get; private set; } = false;

        public bool IsPositionSelected { get; private set; } = false;

        public Item SelectedObject => _selectedObject;

        public int LayerMask => _layerMask;
        
        public Item TemporaryItem => _temporaryItem;

        private void Start()
        {
            _layerMask = 1 << _layer;
            _layerMask = ~_layerMask;
            _itemPositionLooker = GetComponent<ItemPositionLooker>();
        }

        public void DeactivateItem()
        {
            _istemporary = true;
            _temporaryItem = _selectedObject;
            _selectedObject.gameObject.SetActive(false);
            _selectedObject = null;
            _startPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
        }

        public void ActivateItem()
        {
            if (_temporaryItem == null)
                return;

            // _temporaryItem.gameObject.SetActive(true);
            _selectedObject = _temporaryItem;
            _temporaryItem = null;
            _selectedObject.gameObject.SetActive(true);
            _istemporary = false;
            SetItem(_selectedObject, _startPosition);
            // _temporaryItem = null;
        }

        public void SetItem(Item item, ItemPosition itemPosition)
        {
            _selectedObject = item;
            _startPosition = itemPosition;
            _selectedObject.Init(_startPosition);
            _startPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        }

        public void ClearItem()
        {
            _selectedObject = null;
        }

        public void DragItem()
        {
            _itemPositionLooker.LookPosition();
            _selectedObject.ClearPosition();

            if (IsObjectSelected)
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (_objectPlane.Raycast(mouseRay, out _distance))
                {
                    Vector3 mouseWorldPosition = mouseRay.GetPoint(_distance);
                    mouseWorldPosition.y = _selectedObject.transform.position.y;
                    _selectedObject.transform.position = mouseWorldPosition;
                }
            }

            if (IsPositionSelected)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                    {
                        if (itemPosition.IsBusy)
                            return;

                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                        _startPosition = itemPosition;
                        _selectedObject.transform.position = itemPosition.transform.position;
                    }
                }
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
                    Vector3 position = _selectedObject.transform.position;
                    position = new Vector3(position.x, position.y + _offset, position.z);
                    _selectedObject.transform.position = position;
                    _objectPlane = new Plane(Vector3.up, position);
                    IsObjectSelected = true;
                    // IsPositionSelected = true;
                }

                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition) &&
                        !itemPosition.IsBusy)
                    {
                        _objectPlane = new Plane(Vector3.up, _selectedObject.transform.position);
                        // IsObjectSelected = true;
                        IsPositionSelected = true;
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
                    if (!itemPosition.IsBusy && !itemPosition.IsWater)
                    {
                        _selectedObject.transform.position = hit.transform.position;
                        _selectedObject.Init(itemPosition);
                        _selectedObject.Activation();
                        _selectedObject.GetComponent<ItemAnimation>().PositioningAnimation();
                        PlaceChanged?.Invoke();
                        BuildItem?.Invoke(_selectedObject);
                        itemPosition.DeliverObject(_selectedObject);
                        _selectedObject = null;
                        IsObjectSelected = false;
                        IsPositionSelected = false;
                    }
                    else if (itemPosition.IsBusy && IsObjectSelected)
                    {
                        ReturnPosition();
                        itemPosition.Item.GetComponent<ItemAnimation>().BusyPositionAnimation();
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
            _selectedObject.Init(_startPosition);
            IsObjectSelected = false;
            IsPositionSelected = false;
            _startPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        }
    }
}