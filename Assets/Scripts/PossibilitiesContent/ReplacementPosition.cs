using System;
using System.Collections;
using CountersContent;
using Enums;
using ItemContent;
using ItemPositionContent;
using UI.Buttons.BonusesContent;
using UnityEngine;

namespace PossibilitiesContent
{
    public class ReplacementPosition : MonoBehaviour
    {
        [SerializeField] private ReplacementButton _replacementButton;
        [SerializeField] private PossibilitiesCounter _positionsCounter;
        [SerializeField] private AudioSource _audioSource;

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
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);

        public event Action PositionsChanged;

        public event Action PositionChanging;

        public event Action<ItemPosition, ItemPosition> PositionItemsChanging;
        
        public bool IsWorking { get;private  set; }

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

        public void ActivateVisualPosition()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if ((Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                    itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
            }
        }
        
        public void StartReplace()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Replace());
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
                        SetFirstPosition(itemPosition);

                    if (_firstSelect && itemPosition != _firstItemPosition)
                    {
                        if (itemPosition.IsWater && _firstItem.ItemName != Items.LightHouse)
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
                            yield return _waitForSeconds;
                            _secondItemPosition.ReplaceSelectedActivate();
                            _firstItemPosition.ReplaceSelectedActivate();
                            _secondItemPosition.DeliverObject(_firstItem);
                            _firstItemPosition.DeliverObject(_secondItem);
                            CleaningAfterExchange();
                            yield return null;
                            PositionChanging?.Invoke();
                            yield return _waitForSeconds;
                            PositionsChanged?.Invoke();
                            yield return _waitForSeconds;
                            _positionsCounter.DecreaseCount();
                        }
                        else
                        {
                            ChangePosition(_firstItem, _secondItemPosition);
                            _firstItemPosition.ClearingPosition();
                            yield return null;
                            _secondItemPosition.DeliverObject(_firstItem);
                            CleaningAfterExchange();
                            yield return null;
                            PositionChanging?.Invoke();
                            yield return _waitForSeconds;
                            PositionsChanged?.Invoke();
                            yield return _waitForSeconds;
                            _positionsCounter.DecreaseCount();
                        }
                    }
                }
            }
        }

        private void CleaningAfterExchange()
        {
            PositionItemsChanging?.Invoke(_firstItemPosition, _secondItemPosition);
            _firstSelect = false;
            _firstItem = null;
            _firstItemPosition = null;
            _audioSource.PlayOneShot(_audioSource.clip);
        }        
        
        private void SetFirstPosition(ItemPosition itemPosition )
        {
            _firstSelect = true;
            _firstItemPosition = itemPosition;
            _firstItem = itemPosition.Item;
            Vector3 position = _firstItem.transform.position;
            position = new Vector3(position.x, position.y + _offset, position.z);
            _firstItem.ClearPosition();
            _firstItem.transform.position = position;
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
            IsWorking = true;
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

            IsWorking = false;
        }
    }
}