using ItemContent;
using ItemPositionContent;
using Keeper;
using UnityEngine;

namespace Dragger
{
    [RequireComponent(typeof(ItemPositionLooker))]
    public class ItemDragger : MonoBehaviour
    {
        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private Camera _camera;

        private ItemPositionLooker _itemPositionLooker;
        private Plane _objectPlane;
        private float _offset = 1f;
        private int _layer = 3;
        private float _distance;
        private float _distanceToPlane;
        private Vector3 _offsetObject;
        private Vector3 _position;
        private Vector3 _mouseWorldPosition;
        private RaycastHit _hit;
        private Ray _ray;

        public bool IsObjectSelected { get; private set; }

        public bool IsPositionSelected { get; private set; }

        public int LayerMask { get; private set; }

        private void Start()
        {
            LayerMask = 1 << _layer;
            LayerMask = ~LayerMask;
            _itemPositionLooker = GetComponent<ItemPositionLooker>();
        }

        public void DragItem()
        {
            _itemPositionLooker.LookPosition();
            _itemKeeper.SelectedObject.ClearPosition();

            if (IsObjectSelected)
            {
                _ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (_objectPlane.Raycast(_ray, out _distance))
                {
                    _mouseWorldPosition = _ray.GetPoint(_distance);
                    _mouseWorldPosition.y = _itemKeeper.SelectedObject.transform.position.y;
                    _itemKeeper.SelectedObject.transform.position = Vector3.Lerp(
                        _itemKeeper.SelectedObject.transform.position,
                        _mouseWorldPosition + _offsetObject,
                        16 * Time.deltaTime);
                }
            }

            if (IsPositionSelected)
            {
                _ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, LayerMask))
                {
                    if (_hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                    {
                        if (!itemPosition.IsBusy && !itemPosition.IsWater)
                            _itemKeeper.ChangeStartPosition(itemPosition);

                        if (_itemKeeper.SelectedObject.IsLightHouse && !itemPosition.IsBusy)
                            _itemKeeper.ChangeStartPosition(itemPosition);
                    }
                }
            }
        }

        public void SelectItem()
        {
            if (_itemKeeper.SelectedObject == null)
                return;

            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit))
            {
                if (_hit.transform.gameObject.TryGetComponent(out Item item) && !item.IsActive)
                {
                    _position = _itemKeeper.SelectedObject.transform.position;
                    _position = new Vector3(_position.x, _position.y + _offset, _position.z);
                    _itemKeeper.SelectedObject.transform.position = _position;
                    _objectPlane = new Plane(Vector3.up, _position);
                    _objectPlane.Raycast(_ray, out _distanceToPlane);
                    _distance = _distanceToPlane;
                    _mouseWorldPosition = _ray.GetPoint(_distance);
                    _offsetObject = _itemKeeper.SelectedObject.transform.position - _mouseWorldPosition;
                    _offsetObject.y = 0;

                    if (_itemKeeper.SelectedObject.transform.position == _position)
                        IsObjectSelected = true;
                }
                else if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, LayerMask))
                {
                    if (_hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition) &&
                        !itemPosition.IsBusy)
                    {
                        _objectPlane = new Plane(Vector3.up, _itemKeeper.SelectedObject.transform.position);
                        IsPositionSelected = true;
                    }
                }
            }
        }

        public void DisableSelected()
        {
            IsObjectSelected = false;
            IsPositionSelected = false;
        }
    }
}