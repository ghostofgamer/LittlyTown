using System;
using UnityEngine;

public class ItemDragger : MonoBehaviour
{
    [SerializeField] private float _offset = 1.0f;

    private GameObject _selectedObject;
    private Vector3 _originalPosition;
    private bool _isObjectSelected = false;
    private Plane _objectPlane;
    private int _layerMaskIgnore;
    private int _layerMask;
    private int _layer = 3;
    private ItemPosition _currentLookPosition = null;

    public event Action PlaceChanged;

    public event Action PlaceLooking;

    private void Start()
    {
        _layerMask = 1 << _layer;
        _layerMask = ~_layerMask;
        PlaceChanged!.Invoke();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log("попытка1");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                /*if (hit.transform.gameObject.TryGetComponent(out Item item))
                {
                    _selectedObject = hit.transform.gameObject;
                    _originalPosition = _selectedObject.transform.position;
                    _selectedObject.transform.position = new Vector3(
                        _selectedObject.transform.position.x,
                        _selectedObject.transform.position.y + _offset,
                        _selectedObject.transform.position.z);
                    _objectPlane = new Plane(Vector3.up, _selectedObject.transform.position);
                    _isObjectSelected = true;
                }*/
                // Debug.Log("попытка3");
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    _selectedObject.transform.position = new Vector3(
                        _selectedObject.transform.position.x,
                        _selectedObject.transform.position.y + _offset,
                        _selectedObject.transform.position.z);
                    _objectPlane = new Plane(Vector3.up, _selectedObject.transform.position);
                    _isObjectSelected = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (!itemPosition.IsBusy)
                    {
                        // Debug.Log("попытка");
                        _selectedObject.transform.position = hit.transform.position;
                        PlaceChanged?.Invoke();
                        _selectedObject.GetComponent<Item>().Activation();
                        itemPosition.DeliverObject(_selectedObject.GetComponent<Item>());
                        _selectedObject = null;
                        _isObjectSelected = false;
                    }
                    else
                    {
                        _selectedObject.transform.position = _originalPosition;
                        _isObjectSelected = false;
                    }
                }

                else
                {
                    /*_selectedObject.transform.position = _originalPosition;
                    _isObjectSelected = false;*/
                }
            }
            else
            {
                /*_selectedObject.transform.position = _originalPosition;
                _isObjectSelected = false;*/
            }
        }

        if (_isObjectSelected)
        {
            LookItems();

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;

            if (_objectPlane.Raycast(mouseRay, out distance))
            {
                Vector3 mouseWorldPosition = mouseRay.GetPoint(distance);
                mouseWorldPosition.y = _selectedObject.transform.position.y;
                _selectedObject.transform.position = mouseWorldPosition;
            }
        }
    }

    public void SetItem(Item item)
    {
        _selectedObject = item.gameObject;
        _originalPosition = _selectedObject.transform.position;
    }

    private void LookItems()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
        {
            if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
            {
                if (_currentLookPosition == itemPosition)
                {
                    return;
                }

                _currentLookPosition = itemPosition;
                _currentLookPosition.ActivateVisual();
                Debug.Log("Смотрю на позицию");
                PlaceLooking?.Invoke();
            }
        }
    }
}