using System;
using System.Collections;
using System.Collections.Generic;
using Dragger;
using Enums;
using ItemContent;
using ItemPositionContent;
using PossibilitiesContent;
using SpawnContent;
using UnityEngine;

namespace MergeContent
{
    public class LookMerger : MonoBehaviour
    {
        [SerializeField] private ItemPositionLooker _itemPositionLooker;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private Merger _merger;
        [SerializeField] private MinIndexItemSelector _minIndexItemSelector;
        [SerializeField] private AnimationMatches _animationMatches;

        private ItemPosition _currentItemPosition;
        private Item _currentItem;
        private Coroutine _coroutine;
        private List<Item> _matchedItems = new List<Item>();
        private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
        private List<ItemPosition> _positions = new List<ItemPosition>();
        private List<Item> _temporaryItems = new List<Item>();
        private Item _temporaryItem;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.15f);
        private bool _isTryMerge;
        private int _zero = 0;
        private int _targetMinMatchedItems = 2;
        private int _targetMinTemporaryItems = 1;
        private Dictionary<Item, Coroutine> _coroutines = new Dictionary<Item, Coroutine>();
        private Dictionary<Item, List<Item>> _newMatchedItems = new Dictionary<Item, List<Item>>();
        private Dictionary<ItemPosition, ItemPosition> _targetPositions = new Dictionary<ItemPosition, ItemPosition>();
        private Item _item;
        private Coroutine _currentCoroutine;

        public event Action NotMerged;

        public List<ItemMoving> ItemsMoving { get; } = new List<ItemMoving>();

        private void OnEnable()
        {
            _itemPositionLooker.PlaceLooking += OnCheckMatches;
            _spawner.AroundLooking += OnCheckMatches;
        }

        private void OnDisable()
        {
            _itemPositionLooker.PlaceLooking -= OnCheckMatches;
            _spawner.AroundLooking -= OnCheckMatches;
        }

        public void LookAround(ItemPosition itemPosition, Item item)
        {
            _isTryMerge = true;
            SelectItemPositionBeforeLookAround(itemPosition, item);
        }

        private void OnCheckMatches(ItemPosition itemPosition, Item item)
        {
            if (itemPosition.IsWater || itemPosition.IsBusy)
            {
                _animationMatches.StopMoveMatch();
                ItemsMoving.Clear();
                return;
            }

            _isTryMerge = false;
            _targetPositions.Clear();
            SelectItemPositionBeforeLookAround(itemPosition, item);
        }

        private void SelectItemPositionBeforeLookAround(ItemPosition itemPosition, Item item)
        {
            SetValue(itemPosition, item);

            if (itemPosition.IsWater)
                return;

            if (!_targetPositions.ContainsKey(itemPosition))
                _targetPositions.Add(itemPosition, itemPosition);

            _targetPositions[itemPosition].SetSelected();
            LookAtCurrentItem(itemPosition);
        }

        private void LookAtCurrentItem(ItemPosition itemPosition)
        {
            if (_currentItem.ItemName == Items.Crane)
            {
                _temporaryItems.Clear();
                _temporaryItems = _minIndexItemSelector.GetTemporaryItems(_currentItemPosition.ItemPositions);
                _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_currentItemPosition.ItemPositions);

                if (_temporaryItem != null)
                    ChoosePathSearchMatches(_temporaryItem, itemPosition);
                else if (_isTryMerge && _temporaryItem == null)
                    _currentItem.GetComponent<CraneDestroyer>().Destroy();
            }
            else
            {
                ChoosePathSearchMatches(_currentItem, _targetPositions[itemPosition]);
            }
        }

        private void ChoosePathSearchMatches(Item item, ItemPosition itemPosition)
        {
            if (!_isTryMerge)
                _coroutine = StartCoroutine(LookPositions(_currentItemPosition, item));
            else
                FindContainsListsBeforeMerge(item, itemPosition);
        }

        private void FindContainsListsBeforeMerge(Item item, ItemPosition itemPosition)
        {
            if (_coroutines.ContainsKey(item))
            {
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
            }
            else if (!_coroutines.ContainsKey(item))
            {
                if (!_newMatchedItems.ContainsKey(item))
                    _newMatchedItems.Add(item, new List<Item>());

                _currentCoroutine = StartCoroutine(LookMerge(itemPosition, item));
                _coroutines.Add(item, _currentCoroutine);
            }
            else
            {
                if (!_newMatchedItems.ContainsKey(item))
                    _newMatchedItems.Add(item, new List<Item>());

                _currentCoroutine = StartCoroutine(LookMerge(itemPosition, item));
                _coroutines.Add(item, _currentCoroutine);
            }
        }

        private void SetValue(ItemPosition itemPosition, Item item)
        {
            _animationMatches.StopMoveMatch();

            if (_currentItemPosition != null && !_currentItemPosition.IsReplaceSelected)
                _currentItemPosition.ClearingPosition();

            if (!itemPosition.IsBusy && !item.IsLightHouse && itemPosition.IsWater)
                return;

            _currentItemPosition = itemPosition;
            _currentItemPosition.SetSelected();
            _currentItem = item;
            ItemsMoving.Clear();
            ClearLists();

            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator LookPositions(ItemPosition itemPosition, Item item)
        {
            yield return _waitForSeconds;
            ActivationLookPositions(itemPosition, item);
            LookMatchesCount();
        }

        private IEnumerator LookMerge(ItemPosition itemPosition, Item item)
        {
            yield return _waitForSeconds;
            ActivationLookPositions(itemPosition, item);
            SeeHowManyMatchesFound(itemPosition, item);
        }

        private void LookMatchesCount()
        {
            if (_matchedItems.Count >= _targetMinMatchedItems)
            {
                LookAroundNextLevelItem();
            }
            else if (_matchedItems.Count < _targetMinMatchedItems && _temporaryItems.Count > _targetMinTemporaryItems)
            {
                SaveNewTemporaryItem();
                _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _temporaryItem));
            }
            else
            {
                if (ItemsMoving.Count < _targetMinMatchedItems)
                    return;

                if (_targetPositions.ContainsKey(_currentItemPosition))
                    _targetPositions.Remove(_currentItemPosition);

                _animationMatches.StartMoveMatch(_currentItemPosition.transform.position);
            }
        }

        private void LookAroundNextLevelItem()
        {
            if (_currentItem.ItemName == Items.Crane)
                _temporaryItems.Clear();

            _item = _matchedItems[_zero].NextItem;

            foreach (var matchItem in _matchedItems)
                ItemsMoving.Add(matchItem.GetComponent<ItemMoving>());

            _matchedItems.Clear();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if (_targetPositions.ContainsKey(_currentItemPosition))
                _targetPositions.Remove(_currentItemPosition);

            _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _item));
        }

        private void SaveNewTemporaryItem()
        {
            _matchedItems.Clear();
            _temporaryItems.Remove(_temporaryItem);
            _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_temporaryItems);
        }

        private void SeeHowManyMatchesFound(ItemPosition itemPosition, Item item)
        {
            if (_newMatchedItems[item].Count >= _targetMinMatchedItems)
                SendMatchesMerge(itemPosition, item);
            else if (_newMatchedItems[item].Count < _targetMinMatchedItems &&
                     _temporaryItems.Count > _targetMinTemporaryItems)
                SearchAroundTemporaryItem(itemPosition, item);
            else
                StopSearchAround(itemPosition, item);
        }

        private void SendMatchesMerge(ItemPosition itemPosition, Item item)
        {
            _newMatchedItems[item].Add(item);
            _merger.MergeItems(itemPosition, _positions, _newMatchedItems[item], item);

            if (_coroutines.ContainsKey(item))
            {
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
            }

            if (_targetPositions.ContainsKey(itemPosition))
                _targetPositions.Remove(itemPosition);

            if (_temporaryItems.Count > _zero)
                _temporaryItems.Clear();

            if (_newMatchedItems.ContainsKey(item))
            {
                _newMatchedItems[item].Clear();
                _newMatchedItems.Remove(item);
            }
        }

        private void SearchAroundTemporaryItem(ItemPosition itemPosition, Item item)
        {
            if (_coroutines.ContainsKey(_temporaryItem))
            {
                StopCoroutine(_coroutines[_temporaryItem]);
                _coroutines.Remove(_temporaryItem);
            }

            if (_newMatchedItems.ContainsKey(item))
            {
                _newMatchedItems[item].Clear();
                _newMatchedItems.Remove(item);
            }

            SaveNewTemporaryItem();

            if (_coroutines.ContainsKey(_temporaryItem))
            {
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
            }

            _currentCoroutine = StartCoroutine(LookMerge(itemPosition, _temporaryItem));
            _coroutines.Add(_temporaryItem, _currentCoroutine);
        }

        private void StopSearchAround(ItemPosition itemPosition, Item item)
        {
            if (itemPosition.IsReplaceSelected)
            {
                itemPosition.DeactivationSelected();
                itemPosition.ReplaceSelectedDeactivate();
            }

            if (_currentItem.ItemName == Items.Crane)
                _currentItem.GetComponent<CraneDestroyer>().Destroy();

            if (_newMatchedItems.ContainsKey(item))
            {
                _newMatchedItems[item].Clear();
                _newMatchedItems.Remove(item);
            }

            NotMerged?.Invoke();

            if (_targetPositions.ContainsKey(itemPosition))
                _targetPositions.Remove(itemPosition);

            if (_coroutines.ContainsKey(item))
            {
                StopCoroutine(_coroutines[item]);
                _coroutines.Remove(item);
            }
        }

        private void ActivationLookPositions(ItemPosition currentPosition, Item item)
        {
            if (!_newMatchedItems.ContainsKey(item))
                _newMatchedItems.Add(item, new List<Item>());

            if (_checkedPositions.Contains(currentPosition))
                return;

            if (!currentPosition.IsSelected)
            {
                _checkedPositions.Add(currentPosition);
                _matchedItems.Add(currentPosition.Item);
                _positions.Add(currentPosition);

                if (_isTryMerge)
                {
                    ItemsMoving.Add(currentPosition.Item.GetComponent<ItemMoving>());
                    _newMatchedItems[item].Add(currentPosition.Item);
                }
            }

            foreach (var arroundPosition in currentPosition.ItemPositions)
            {
                if (arroundPosition == null)
                    continue;

                if (arroundPosition.Item != null && arroundPosition.Item.NextItem != null &&
                    arroundPosition.Item.ItemName.Equals(item.ItemName))
                    ActivationLookPositions(arroundPosition, item);
            }
        }

        private void ClearLists()
        {
            ItemsMoving.Clear();
            _matchedItems.Clear();
            _checkedPositions.Clear();
            _positions.Clear();
        }
    }
}