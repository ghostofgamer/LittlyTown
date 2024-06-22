using System;
using System.Collections;
using CountersContent;
using Dragger;
using Enums;
using ItemContent;
using ItemPositionContent;
using UI.Buttons;
using UI.Buttons.BonusesContent;
using UnityEngine;

public class ReplacementPosition : MonoBehaviour
{
    [SerializeField] private PositionMatcher _positionMatcher;
    [SerializeField] private ReplacementButton _replacementButton;
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private PossibilitiesCounter _positionsCounter;
    [SerializeField] private LookMerger _lookMerger;
    [SerializeField]private AudioSource _audioSource;

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

    public event Action PositionsChanged;

    public event Action PositionChanging;
    
    public event Action<ItemPosition,ItemPosition> PositionItemsChanging;

    private void OnEnable()
    {
        _replacementButton.ReplaceActivated += ActivateWork;
        _replacementButton.ReplaceDeactivated += DeactivateWork;
    }

    private void OnDisable()
    {
        _replacementButton.ReplaceActivated -= ActivateWork;
        _replacementButton.ReplaceDeactivated -= DeactivateWork;
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

        if (Input.GetMouseButtonUp(0))
        {
            _isLooking = false;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Replace());
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
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                    else
                    {
                        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    }
                }
            }
        }
    }

    private IEnumerator Replace()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
        {
            if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
            {
                if (!_firstSelect && itemPosition.IsBusy && itemPosition.Item != null)
                {
                    _firstSelect = true;
                    _firstItemPosition = itemPosition;
                    _firstItem = itemPosition.Item;
                    Vector3 position = _firstItem.transform.position;
                    position = new Vector3(position.x, position.y + _offset, position.z);
                    _firstItem.ClearPosition();
                    _firstItem.transform.position = position;
                }

                if (_firstSelect && itemPosition != _firstItemPosition)
                {
                    if (itemPosition.IsWater &&_firstItem.ItemName != Items.LightHouse)
                        yield break;

                    if (_firstItem.ItemName == Items.LightHouse && !itemPosition.IsWater)
                        yield break;

                    _secondItemPosition = itemPosition;

                    if (itemPosition.Item != null)
                    {
                        _secondItem = itemPosition.Item;
                        ChangePosition(_firstItem, _secondItemPosition);
                        ChangePosition(_secondItem, _firstItemPosition);
                        yield return null;
                        yield return new WaitForSeconds(0.1f);
                        _secondItemPosition.ReplaceSelectedActivate();
                        _firstItemPosition.ReplaceSelectedActivate();
                        _secondItemPosition.DeliverObject(_firstItem);
                        _firstItemPosition.DeliverObject(_secondItem);
                        PositionItemsChanging?.Invoke(_firstItemPosition,_secondItemPosition);
                        _firstSelect = false;
                        _firstItem = null;
                        _firstItemPosition = null;
                        _audioSource.PlayOneShot(_audioSource.clip);
                        yield return null;
                        PositionChanging?.Invoke();
                        yield return new WaitForSeconds(0.1f);
                        PositionsChanged?.Invoke();
                        yield return new WaitForSeconds(0.1f);
                        _positionsCounter.DecreaseCount();
                    }
                    else
                    {
                        ChangePosition(_firstItem, _secondItemPosition);
                        _firstItemPosition.ClearingPosition();
                        yield return null;
                        _secondItemPosition.DeliverObject(_firstItem);
                        PositionItemsChanging?.Invoke(_firstItemPosition,_secondItemPosition);
                        _firstSelect = false;
                        _firstItem = null;
                        _firstItemPosition = null;
                        _audioSource.PlayOneShot(_audioSource.clip);
                        yield return null;
                        PositionChanging?.Invoke();
                        yield return new WaitForSeconds(0.1f);
                        PositionsChanged?.Invoke();
                        yield return new WaitForSeconds(0.1f);
                        _positionsCounter.DecreaseCount();
                    }
                }
            }
        }
    }

    private void ChangePosition(Item item, ItemPosition itemPosition)
    {
        item.ClearPosition();
        item.gameObject.SetActive(false);
        item.transform.position = itemPosition.transform.position;
        item.gameObject.SetActive(true);
        item.Init(itemPosition);
        item.Activation();
        item.GetComponent<ItemAnimation>().PositioningAnimation();
    }

    private void ActivateWork()
    {
        _isWorking = true;
        // _itemDragger.DeactivateItem();
    }

    private void DeactivateWork()
    {
        if (_firstSelect)
        {
            _firstItem.Init(_firstItemPosition);
            _firstItem.transform.position = _firstItemPosition.transform.position;
            _firstSelect = false;
            _firstItem = null;
            _firstItemPosition = null;
        }

        _isWorking = false;
        // _itemDragger.ActivateItem();
    }
}