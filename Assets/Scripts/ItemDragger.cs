using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragger : MonoBehaviour
{
    [SerializeField] private float _offset = 1.0f;
    [SerializeField] private LayerMask itemLayer;

    private GameObject _selectedObject;
    private Vector3 _originalPosition;
    private bool _isObjectSelected = false;
    private Plane _objectPlane;
    private int _layerMaskIgnore;
    private int _layerMask;
    private int _layer = 3;
    
    private void Start()
    {
        _layerMaskIgnore = ~(1 << itemLayer.value); 
        Debug.Log("игнор " + _layerMaskIgnore);
        _layerMask = 1 << _layer;
        _layerMask = ~_layerMask;
        Debug.Log("игнори " + _layerMask);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.TryGetComponent(out Item item))
                {
                    _selectedObject = hit.transform.gameObject;
                    _originalPosition = _selectedObject.transform.position;
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
            Debug.Log("жмяк");

            RaycastHit dropHit;
            Ray dropRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            
            /*if (Physics.Raycast(dropRay, out dropHit, Mathf.Infinity, _layerMaskIgnore))
            {
                Debug.Log(dropHit.collider.gameObject.name);

                if (dropHit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    _selectedObject.transform.position = dropHit.transform.position;
                }
                else
                    _selectedObject.transform.position = _originalPosition;
            }

            else
            {
                Debug.Log("dropHit.collider.gameObject.name");
            }*/



            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                Debug.Log(hit.collider.gameObject.name);

                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    _selectedObject.transform.position = hit.transform.position;
                }
                else
                    _selectedObject.transform.position = _originalPosition;
            }

            _isObjectSelected = false;
        }
        
        if (_isObjectSelected)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (_objectPlane.Raycast(mouseRay, out distance))
            {
                Vector3 mouseWorldPosition = mouseRay.GetPoint(distance);
                mouseWorldPosition.y = _selectedObject.transform.position.y; // Замораживаем вертикальное движение
                _selectedObject.transform.position = mouseWorldPosition;
            }
            
            
            
            /*Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane; // Устанавливаем значение z для точности луча
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // mouseWorldPosition.y = selectedObject.transform.position.y; // Замораживаем вертикальное движение
            selectedObject.transform.position = mouseWorldPosition;*/
        }
    }
}