using System;
using System.Collections;
using Dragger;
using ItemContent;
using ItemPositionContent;
using UI.Buttons;
using UnityEngine;

public class RemovalItems : MonoBehaviour
{
    [SerializeField] private BuldozerButton _buldozerButton;
    [SerializeField] private ItemDragger _itemDragger;

    private bool _isWorking;
    private bool _isLooking;
    private int _layerMask;
    private int _layer = 3;

    public event Action ItemRemoved;
    
    private void OnEnable()
    {
        _buldozerButton.RemovalActivated += ActivateWork;
        _buldozerButton.RemovalDeactivated += DeactivateWork;
    }

    private void OnDisable()
    {
        _buldozerButton.RemovalActivated -= ActivateWork;
        _buldozerButton.RemovalDeactivated -= DeactivateWork;
    }

    private void Start()
    {
        _layerMask = 1 << _layer;
        _layerMask = ~_layerMask;
    }

    private void Update()
    {
        if (!_isWorking)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _isLooking = true;
        }

        if (_isLooking)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (itemPosition.IsBusy && !itemPosition.IsWater)
                    {
                        itemPosition.Item.GetComponent<ItemAnimation>().BusyPositionAnimation();
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                    else
                    {
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isLooking = false;
            StartCoroutine(Remove());
            
            /*RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition) && itemPosition.IsBusy)
                {
                    itemPosition.Item.Deactivation();
                    itemPosition.Item.gameObject.SetActive(false);
                    itemPosition.ClearingPosition();
                    Debug.Log("Remove");
                    // DeactivateWork();
                    ItemRemoved?.Invoke();
                }
            }*/

            /*Debug.Log("нажатие");
            _isWorking = false;
            _itemDragger.ActivateItem();*/
        }
    }

    private IEnumerator Remove()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
        {
            if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition) && itemPosition.IsBusy)
            {
                itemPosition.Item.Deactivation();
                itemPosition.Item.gameObject.SetActive(false);
                itemPosition.ClearingPosition();
                Debug.Log("Remove");
                // DeactivateWork();
                yield return null;
                ItemRemoved?.Invoke();
            }
        }
    }
    
    private void ActivateWork()
    {
        Debug.Log("ACTIVEWORK");
        _isWorking = true;
        _itemDragger.DeactivateItem();
    }

    private void DeactivateWork()
    {
        Debug.Log("DeACTIVEWORK");
        _isWorking = false;
        _itemDragger.ActivateItem();
    }
}