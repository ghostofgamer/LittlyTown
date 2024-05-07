using System;
using Dragger;
using ItemContent;
using ItemPositionContent;
using UI.Buttons;
using UnityEngine;

public class ReplacementPosition : MonoBehaviour
{
    [SerializeField] private ReplacementPositionButton _replacementPositionButton;
    [SerializeField] private ItemDragger _itemDragger;

    private bool _isWorking;
    private bool _isLooking;
    private int _layerMask;
    private int _layer = 3;
    private Coroutine _coroutine;
    private Item _selectedObject;
    private float _offset = 3f;
    private bool _firstSelect = false;
    private ItemPosition _firstItemPosition;
    private ItemPosition _secondItemPosition;
    private ItemPosition _temporaryItemPosition;
    private Item _firstItem;
    private Item _secondItem;
    private Item _temporaryItem;

    private void OnEnable()
    {
        _replacementPositionButton.ReplaceActivated += ActivateWork;
        _replacementPositionButton.ReplaceDeactivated += DeactivateWork;
    }

    private void OnDisable()
    {
        _replacementPositionButton.ReplaceActivated -= ActivateWork;
        _replacementPositionButton.ReplaceDeactivated -= DeactivateWork;
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
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (itemPosition.IsBusy)
                    {
                        if (!_firstSelect)
                        {
                            _firstSelect = true;
                            _firstItemPosition = itemPosition;
                            _firstItem = itemPosition.Item;
                            Vector3 position = _firstItem.transform.position;
                            position = new Vector3(position.x, position.y + _offset, position.z);
                            _firstItem.transform.position = position;
                        }

                        if (_firstSelect && itemPosition != _firstItemPosition)
                        {
                            Debug.Log("Secon");
                            if (itemPosition.Item != null)
                            {
                                Debug.Log("Меняемся местами");
                                _secondItemPosition = itemPosition;
                                _secondItem = itemPosition.Item;
                                _firstItem.ClearPosition();
                                _secondItem.ClearPosition();
                                _secondItem.gameObject.SetActive(false);
                                _firstItem.gameObject.SetActive(false);
                                _secondItem.transform.position = _firstItemPosition.transform.position;
                                _firstItem.transform.position = _secondItemPosition.transform.position;
                                _secondItem.gameObject.SetActive(true);
                                _firstItem.gameObject.SetActive(true);
                                _firstItem.Init(_secondItemPosition);
                                _secondItem.Init(_firstItemPosition);
                                _firstItem.Activation();
                                _secondItem.Activation();
                                _firstItem.GetComponent<ItemAnimation>().PositioningAnimation();
                                _secondItem.GetComponent<ItemAnimation>().PositioningAnimation();
                                _firstSelect = false;
                                _firstItem = null;
                                _firstItemPosition = null;
                            }
                        }
                    }

                    if (_firstSelect && itemPosition != _firstItemPosition)
                    {
                        if (!itemPosition.IsBusy)
                        {
                            Debug.Log("Меняемся на пустое");
                            _secondItemPosition = itemPosition;
                            _firstItem.ClearPosition();
                            _firstItem.gameObject.SetActive(false);
                            _firstItem.transform.position = _secondItemPosition.transform.position;
                            _firstItemPosition.ClearingPosition();
                            _firstItem.gameObject.SetActive(true);
                            _firstItem.Init(_secondItemPosition);
                            _firstItem.Activation();
                            _firstItem.GetComponent<ItemAnimation>().PositioningAnimation();
                            _firstSelect = false;
                            _firstItem = null;
                            _firstItemPosition = null;
                        }
                    }
                }
            }
        }


        /*
        if (Input.GetMouseButtonDown(0))
        {
            _isLooking = true;

            RaycastHit hits;
            Ray rays = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rays, out hits))
            {
                if (hits.transform.gameObject.TryGetComponent(out Item item) && item.IsActive)
                {
                    Debug.Log("item " + item);
                    Vector3 position = item.transform.position;
                    position = new Vector3(position.x, position.y + _offset, position.z);
                    item.transform.position = position;
                    // _objectPlane = new Plane(Vector3.up, position);
                }
            }
        }*/

        /*if (_isLooking)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (itemPosition.IsBusy && !itemPosition.IsWater)
                    {
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                    else
                    {
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                }
            }
        }*/
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