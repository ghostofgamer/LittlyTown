using System;
using System.Collections;
using Enums;
using ItemContent;
using ItemPositionContent;
using Unity.VisualScripting;
using UnityEngine;

namespace Dragger
{
    public class ItemDragger : MonoBehaviour
    {
        [SerializeField] private Spawner _spawner;
        [SerializeField] private ItemsStorage _itemStorage;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private LookMerger _lookMerger;

        private ItemPositionLooker _itemPositionLooker;
        private ItemPosition _startPosition;
        private Item _selectedObject;
        private Plane _objectPlane;
        private float _offset = 1f;
        private int _layerMaskIgnore;
        private int _layerMask;
        private int _layer = 3;
        private float _distance;
        private Item _temporaryItem;
        private bool _istemporary;


        private Vector3 _offsetObject;

        public event Action PlaceChanged;

        public event Action<ItemPosition> StepCompleted;

        public event Action<Item> BuildItem;

        public event Action<Item, Item> SelectItemReceived;

        public event Action SelectNewItem;

        public bool IsObjectSelected { get; private set; } = false;

        public bool IsPositionSelected { get; private set; } = false;

        public Item SelectedObject => _selectedObject;

        public int LayerMask => _layerMask;

        public Item TemporaryItem => _temporaryItem;

        private ItemPosition _lastTrowPosition;


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
            _layerMask = 1 << _layer;
            _layerMask = ~_layerMask;
            _itemPositionLooker = GetComponent<ItemPositionLooker>();
        }

        public void SetTemporaryItem(Item item)
        {
            _temporaryItem = _selectedObject;
            _temporaryItem.gameObject.SetActive(false);
            SetItem(item, _startPosition);
            _selectedObject.gameObject.SetActive(true);

            /*Debug.Log("Temporary " + _temporaryItem);
            Debug.Log("_selectedObject " + _selectedObject);*/
        }

        public void ClearTemporaryItem()
        {
            _temporaryItem = null;
        }

        public void SetTemporaryObject(Item item)
        {
            _temporaryItem = item;

            if (_temporaryItem != null)
                _temporaryItem.gameObject.SetActive(false);
        }

        public void SetItem(Item item, ItemPosition itemPosition)
        {
            // Debug.Log("1");

            _selectedObject = item;
            // Debug.Log("3");
            _startPosition = itemPosition;
            // Debug.Log("5");
            _selectedObject.Init(_startPosition);
            /*Debug.Log("6" + item.name );
            Debug.Log("6" + itemPosition.name );*/
            // Debug.Log(_startPosition);
            _startPosition.GetComponent<VisualItemPosition>().ActivateVisual();
            // Debug.Log("Dragger");
            SelectItemReceived?.Invoke(_selectedObject, _temporaryItem);
            SelectNewItem?.Invoke();
        }

        public void ClearAll()
        {
            if (_selectedObject != null)
            {
                _selectedObject.gameObject.SetActive(false);
                _selectedObject = null;
            }

            if (_temporaryItem != null)
            {
                _temporaryItem.gameObject.SetActive(false);
                _temporaryItem = null;
            }

            if (_startPosition != null)
            {
                _startPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
                _startPosition = null;
            }
        }

        public void SwitchOff()
        {
            if (_selectedObject != null)
                _selectedObject.gameObject.SetActive(false);

            if (_startPosition != null)
                _startPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
        }

        public void SwitchOn()
        {
            // Debug.Log("Switch");
            if (_selectedObject != null)
                _selectedObject.gameObject.SetActive(true);

            if (_startPosition != null)
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


// Сохраняем оффсет между объектом и курсором по оси XZ
                    /*Vector3 offset = _selectedObject.transform.position - mouseWorldPosition;
                    Debug.Log("OFFSET " + offset );
                    offset.y = 0; // Ограничиваем оффсет по оси Y
                    Debug.Log("OFFSET Y" + offset );
                    
                    Debug.Log("MousePos " + mouseWorldPosition );
                    Debug.Log("MousePos  Offset " + mouseWorldPosition + offset);*/
// Передвигаем объект по оси XZ с сохраненным оффсетом

                    // _selectedObject.transform.position = mouseWorldPosition + _offsetObject;

                    /*Debug.Log("_selectedObject.transform.position ТУТ " +  _selectedObject.transform.position);
                    Debug.Log("mouseWorldPosition " +  mouseWorldPosition + _offsetObject);*/

                    // Debug.Log("Selected " + _selectedObject.transform.position);


                    /*Vector3 mouseWorldPosition = mouseRay.GetPoint(_distance);
                    mouseWorldPosition.y = _selectedObject.transform.position.y;
                    _selectedObject.transform.position = mouseWorldPosition;*/


                    _selectedObject.transform.position = Vector3.Lerp(_selectedObject.transform.position,
                        mouseWorldPosition + _offsetObject, 16 * Time.deltaTime);
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
                        if (!itemPosition.IsBusy && !itemPosition.IsWater)
                            ChangeStartPosition(itemPosition);

                        if (_selectedObject.IsLightHouse && !itemPosition.IsBusy)
                            ChangeStartPosition(itemPosition);
                    }
                }
            }
        }

        private void ChangeStartPosition(ItemPosition itemPosition)
        {
            itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
            _startPosition = itemPosition;
            _selectedObject.transform.position = itemPosition.transform.position;
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
                    float distanceToPlane;
                    _objectPlane.Raycast(ray, out distanceToPlane);
                    _distance = distanceToPlane;

                    Vector3 mouseWorldPosition = ray.GetPoint(_distance);
                    _offsetObject = _selectedObject.transform.position - mouseWorldPosition;
                    _offsetObject.y = 0;

                    /*Debug.Log("_offsetObject " + _offsetObject);
                    Debug.Log("Distance " + _distance);*/

                    if (_selectedObject.transform.position == position)
                        IsObjectSelected = true;
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
                    if (!itemPosition.IsBusy && !itemPosition.IsWater && !_selectedObject.IsLightHouse)
                    {
                        StepCompleted?.Invoke(itemPosition);
                        _selectedObject.transform.position = hit.transform.position;
                        _selectedObject.Init(itemPosition);
                        _selectedObject.Activation();
                        _audioSource.PlayOneShot(_audioSource.clip);
                        _selectedObject.GetComponent<ItemAnimation>().PositioningAnimation();
                        BuildItem?.Invoke(_selectedObject);
                        itemPosition.DeliverObject(_selectedObject);
                        // Debug.Log("Place1");
                        PlaceChanged?.Invoke();
                        if (_selectedObject.ItemName == Items.LightHouse||_selectedObject.ItemName == Items.Crane)
                            StartCoroutine(Continue(itemPosition));
                        _selectedObject = null;
                        _lastTrowPosition = itemPosition;


                        IsObjectSelected = false;
                        IsPositionSelected = false;
                    }
                    else if (!itemPosition.IsBusy && itemPosition.IsWater && _selectedObject.IsLightHouse)
                    {
                        StepCompleted?.Invoke(itemPosition);
                        // Debug.Log("Water");
                        _selectedObject.transform.position = hit.transform.position;
                        _selectedObject.Init(itemPosition);
                        _selectedObject.Activation();
                        _audioSource.PlayOneShot(_audioSource.clip);
                        _selectedObject.GetComponent<ItemAnimation>().PositioningAnimation();
                        itemPosition.DeliverObject(_selectedObject);
                        BuildItem?.Invoke(_selectedObject);
                        // Debug.Log("Place3");
                        PlaceChanged?.Invoke();
                        if (_selectedObject.ItemName == Items.LightHouse||_selectedObject.ItemName == Items.Crane)
                            StartCoroutine(Continue(itemPosition));
                        _selectedObject = null;
                        _lastTrowPosition = itemPosition;


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

        private void GetItemForLastPosition()
        {
            StartCoroutine(Continue(_lastTrowPosition));
        }

        private IEnumerator Continue(ItemPosition itemPosition)
        {
            yield return new WaitForSeconds(0.1f);

            if (_temporaryItem != null)
            {
                if (itemPosition == _startPosition)
                {
                    ItemPosition newPosition = _spawner.GetPosition();

                    if (newPosition != null)
                    {
                        SetItem(_temporaryItem, newPosition);
                        _temporaryItem = null;
                        _selectedObject.gameObject.SetActive(true);
                    }

                    // Debug.Log("ЗАНЯТА ");
                }
                else
                {
                    // Debug.Log("СВОБОДНА");
                    SetItem(_temporaryItem, _startPosition);
                    _temporaryItem = null;
                    _selectedObject.gameObject.SetActive(true);
                }

                /*_temporaryItem = null;
                _selectedObject.gameObject.SetActive(true);*/
            }
        }
    }
}