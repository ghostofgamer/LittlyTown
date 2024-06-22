using System;
using System.Collections;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

namespace Dragger
{
    public class ItemDragger : MonoBehaviour
    {
        [SerializeField] private Spawner _spawner;
        [SerializeField] private LookMerger _lookMerger;
        [SerializeField] private ItemThrower _itemThrower;

        private ItemPositionLooker _itemPositionLooker;
        private Plane _objectPlane;
        private float _offset = 1f;
        private int _layerMaskIgnore;
        private int _layer = 3;
        private float _distance;
        private bool _isTemporary;
        private Vector3 _offsetObject;

        public event Action SelectNewItem;

        public ItemPosition StartPosition { get; private set; }

        public bool IsObjectSelected { get; private set; } = false;

        public bool IsPositionSelected { get; private set; } = false;

        public Item SelectedObject { get; private set; }

        public int LayerMask { get; private set; }

        public Item TemporaryItem { get; private set; }
        
        private void OnEnable()
        {
            _lookMerger.NotMerged += GetItemForLastPosition;
        }

        private void OnDisable()
        {
            _lookMerger.NotMerged -= GetItemForLastPosition;
        }

        private void Start()
        {
            LayerMask = 1 << _layer;
            LayerMask = ~LayerMask;
            _itemPositionLooker = GetComponent<ItemPositionLooker>();
        }

        public void SetTemporaryItem(Item item)
        {
            TemporaryItem = SelectedObject;
            TemporaryItem.gameObject.SetActive(false);
            SetItem(item, StartPosition);
            SelectedObject.gameObject.SetActive(true);
        }

        public void ClearTemporaryItem()
        {
            TemporaryItem = null;
        }

        public void SetTemporaryObject(Item item)
        {
            TemporaryItem = item;

            if (TemporaryItem != null)
                TemporaryItem.gameObject.SetActive(false);
        }

        public void SetItem(Item item, ItemPosition itemPosition)
        {
            SelectedObject = item;
            StartPosition = itemPosition;
            SelectedObject.Init(StartPosition);
            StartPosition.GetComponent<VisualItemPosition>().ActivateVisual();
            SelectNewItem?.Invoke();
        }

        public void ClearAll()
        {
            if (SelectedObject != null)
            {
                SelectedObject.gameObject.SetActive(false);
                SelectedObject = null;
            }

            if (TemporaryItem != null)
            {
                TemporaryItem.gameObject.SetActive(false);
                TemporaryItem = null;
            }

            if (StartPosition != null)
            {
                StartPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
                StartPosition = null;
            }
        }

        public void SwitchOff()
        {
            if (SelectedObject != null)
                SelectedObject.gameObject.SetActive(false);

            if (StartPosition != null)
                StartPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
        }

        public void SwitchOn()
        {
            if (SelectedObject != null)
                SelectedObject.gameObject.SetActive(true);

            if (StartPosition != null)
                StartPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        }

        public void ClearItem()
        {
            SelectedObject = null;
        }

        public void DragItem()
        {
            _itemPositionLooker.LookPosition();
            SelectedObject.ClearPosition();

            if (IsObjectSelected)
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (_objectPlane.Raycast(mouseRay, out _distance))
                {
                    Vector3 mouseWorldPosition = mouseRay.GetPoint(_distance);
                    mouseWorldPosition.y = SelectedObject.transform.position.y;
                    SelectedObject.transform.position = Vector3.Lerp(SelectedObject.transform.position,
                        mouseWorldPosition + _offsetObject, 16 * Time.deltaTime);
                }
            }

            if (IsPositionSelected)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask))
                {
                    if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                    {
                        if (!itemPosition.IsBusy && !itemPosition.IsWater)
                            ChangeStartPosition(itemPosition);

                        if (SelectedObject.IsLightHouse && !itemPosition.IsBusy)
                            ChangeStartPosition(itemPosition);
                    }
                }
            }
        }

        private void ChangeStartPosition(ItemPosition itemPosition)
        {
            itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
            StartPosition = itemPosition;
            SelectedObject.transform.position = itemPosition.transform.position;
        }

        public void SelectItem()
        {
            if (SelectedObject == null)
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.TryGetComponent(out Item item) && !item.IsActive)
                {
                    Vector3 position = SelectedObject.transform.position;
                    position = new Vector3(position.x, position.y + _offset, position.z);
                    SelectedObject.transform.position = position;
                    _objectPlane = new Plane(Vector3.up, position);
                    float distanceToPlane;
                    _objectPlane.Raycast(ray, out distanceToPlane);
                    _distance = distanceToPlane;
                    Vector3 mouseWorldPosition = ray.GetPoint(_distance);
                    _offsetObject = SelectedObject.transform.position - mouseWorldPosition;
                    _offsetObject.y = 0;

                    if (SelectedObject.transform.position == position)
                        IsObjectSelected = true;
                }

                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask))
                {
                    if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition) &&
                        !itemPosition.IsBusy)
                    {
                        _objectPlane = new Plane(Vector3.up, SelectedObject.transform.position);
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

        public void GetItemForLastPosition()
        {
            StartCoroutine(Continue(_itemThrower.LastTrowPosition));
        }

        private IEnumerator Continue(ItemPosition itemPosition)
        {
            yield return new WaitForSeconds(0.1f);

            if (TemporaryItem != null)
            {
                if (itemPosition == StartPosition)
                {
                    ItemPosition newPosition = _spawner.GetPosition();

                    if (newPosition != null)
                    {
                        SetItem(TemporaryItem, newPosition);
                        TemporaryItem = null;
                        SelectedObject.gameObject.SetActive(true);
                    }
                }
                else
                {
                    SetItem(TemporaryItem, StartPosition);
                    TemporaryItem = null;
                    SelectedObject.gameObject.SetActive(true);
                }
            }
        }
    }
}