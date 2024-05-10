using System;
using System.Collections;
using Dragger;
using ItemContent;
using ItemPositionContent;
using UI.Buttons;
using UnityEngine;

public class RemovalItems : MonoBehaviour
{
    [SerializeField] private BulldozerButton _bulldozerButton;
    [SerializeField] private ItemDragger _itemDragger;

    private bool _isWorking;
    private bool _isLooking;
    private int _layerMask;
    private int _layer = 3;
    private Coroutine _coroutine;

    public event Action ItemRemoved;

    private void OnEnable()
    {
        _bulldozerButton.RemovalActivated += ActivateWork;
        _bulldozerButton.RemovalDeactivated += DeactivateWork;
    }

    private void OnDisable()
    {
        _bulldozerButton.RemovalActivated -= ActivateWork;
        _bulldozerButton.RemovalDeactivated -= DeactivateWork;
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

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Remove());
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
                yield return null;
                ItemRemoved?.Invoke();
            }
        }
    }

    private void ActivateWork()
    {
        _isWorking = true;
        _itemDragger.DeactivateItem();
    }

    private void DeactivateWork()
    {
        _isWorking = false;
        _itemDragger.ActivateItem();
    }
}