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

        private ItemPosition _currentItemPosition;
        private Item _currentItem;
        private Coroutine _coroutine;
        private List<Item> _matchedItems = new List<Item>();
        private List<Item> _completeList = new List<Item>();
        private List<ItemPosition> _checkedPositions = new List<ItemPosition>();
        private List<ItemPosition> _positions = new List<ItemPosition>();
        private List<Item> _temporaryItems = new List<Item>();
        private Item _temporaryItem;
        private bool _isTryMerge;
        private Dictionary<Item, Coroutine> _coroutines = new Dictionary<Item, Coroutine>();
        private Dictionary<Item, List<Item>> _newMatchedItems = new Dictionary<Item, List<Item>>();
        private Dictionary<ItemPosition, ItemPosition> _targetPosition = new Dictionary<ItemPosition, ItemPosition>();

        public event Action NotMerged;

        private void OnEnable()
        {
            _itemPositionLooker.PlaceLooking += CheckMatches;
            _spawner.LooksNeighbors += CheckMatches;
        }

        private void OnDisable()
        {
            _itemPositionLooker.PlaceLooking -= CheckMatches;
            _spawner.LooksNeighbors -= CheckMatches;
        }

        public void LookAround(ItemPosition itemPosition, Item item)
        {
            _isTryMerge = true;
            CheckCoroutine(itemPosition, item);
        }

        public void StopMoveMatch()
        {
            foreach (var matchItem in _completeList)
                matchItem.GetComponent<ItemMoving>().StopMove();
        }

        private void CheckMatches(ItemPosition itemPosition, Item item)
        {
            if (itemPosition.IsWater || itemPosition.IsBusy)
            {
                StopMoveMatch();
                _completeList.Clear();
                return;
            }

            _isTryMerge = false;
            _targetPosition.Clear();
            CheckCoroutine(itemPosition, item);
        }

        private void CheckCoroutine(ItemPosition itemPosition, Item item)
        {
            SetValue(itemPosition, item);

            if (itemPosition.IsWater)
                return;

            if (!_targetPosition.ContainsKey(itemPosition))
                _targetPosition.Add(itemPosition, itemPosition);

            _targetPosition[itemPosition].SetSelected();

            if (_currentItem.ItemName == Items.Crane)
            {
                _temporaryItems.Clear();
                _temporaryItems = _minIndexItemSelector.GetTemporaryItems(_currentItemPosition.ItemPositions);
                _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_currentItemPosition.ItemPositions);

                if (_temporaryItem != null)
                    CheckStartCoroutine(_temporaryItem, itemPosition);
                else if (_isTryMerge && _temporaryItem == null)
                    _currentItem.GetComponent<CraneDestroyer>().Destroy();
            }
            else
            {
                CheckStartCoroutine(_currentItem, _targetPosition[itemPosition]);
            }
        }

        private void CheckStartCoroutine(Item item, ItemPosition itemPosition)
        {
            if (!_isTryMerge)
            {
                _coroutine = StartCoroutine(LookPositions(_currentItemPosition, item));
            }
            else
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

                    Coroutine coroutine = StartCoroutine(LookMerge(itemPosition, item));
                    _coroutines.Add(item, coroutine);
                }
                else
                {
                    if (!_newMatchedItems.ContainsKey(item))
                        _newMatchedItems.Add(item, new List<Item>());

                    Coroutine coroutine = StartCoroutine(LookMerge(itemPosition, item));
                    _coroutines.Add(item, coroutine);
                }
            }
        }

        private void SetValue(ItemPosition itemPosition, Item item)
        {
            StopMoveMatch();

            if (_currentItemPosition != null && !_currentItemPosition.IsReplaceSelected)
                _currentItemPosition.ClearingPosition();

            if (!itemPosition.IsBusy && !item.IsLightHouse && itemPosition.IsWater)
                return;

            _currentItemPosition = itemPosition;
            _currentItemPosition.SetSelected();
            _currentItem = item;
            _completeList.Clear();
            ClearLists();

            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator LookPositions(ItemPosition itemPosition, Item item)
        {
            yield return new WaitForSeconds(0.15f);
            ActivationLookPositions(itemPosition, item);
            CheckNextLevel();
        }

        private IEnumerator LookMerge(ItemPosition itemPosition, Item item)
        {
            yield return new WaitForSeconds(0.15f);
            ActivationLookPositions(itemPosition, item);
            CheckMatchMerge(itemPosition, item);
        }

        private void CheckNextLevel()
        {
            if (_matchedItems.Count >= 2)
            {
                if (_currentItem.ItemName == Items.Crane)
                    _temporaryItems.Clear();

                Item item = _matchedItems[0].NextItem;

                foreach (var matchItem in _matchedItems)
                    _completeList.Add(matchItem);

                _matchedItems.Clear();

                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                if (_targetPosition.ContainsKey(_currentItemPosition))
                    _targetPosition.Remove(_currentItemPosition);

                _coroutine = StartCoroutine(LookPositions(_currentItemPosition, item));
            }
            else if (_matchedItems.Count < 2 && _temporaryItems.Count > 1)
            {
                SaveNewTemporaryItem();
                _coroutine = StartCoroutine(LookPositions(_currentItemPosition, _temporaryItem));
            }
            else
            {
                if (_completeList.Count < 2)
                    return;

                if (_targetPosition.ContainsKey(_currentItemPosition))
                    _targetPosition.Remove(_currentItemPosition);

                foreach (var itemMatch in _completeList)
                    itemMatch.GetComponent<ItemMoving>().MoveCyclically(_currentItemPosition.transform.position);
            }
        }

        private void SaveNewTemporaryItem()
        {
            _matchedItems.Clear();
            _temporaryItems.Remove(_temporaryItem);
            _temporaryItem = _minIndexItemSelector.GetItemMinIndex(_temporaryItems);
        }

        private void CheckMatchMerge(ItemPosition itemPosition, Item item)
        {
            if (_newMatchedItems[item].Count >= 2)
            {
                _newMatchedItems[item].Add(item);
                _merger.MergeItems(itemPosition, _positions, _newMatchedItems[item], item);

                if (_coroutines.ContainsKey(item))
                {
                    StopCoroutine(_coroutines[item]);
                    _coroutines.Remove(item);
                }

                if (_targetPosition.ContainsKey(itemPosition))
                    _targetPosition.Remove(itemPosition);

                if (_temporaryItems.Count > 0)
                    _temporaryItems.Clear();

                if (_newMatchedItems.ContainsKey(item))
                {
                    _newMatchedItems[item].Clear();
                    _newMatchedItems.Remove(item);
                }
            }
            else if (_newMatchedItems[item].Count < 2 && _temporaryItems.Count > 1)
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

                Coroutine coroutine = StartCoroutine(LookMerge(itemPosition, _temporaryItem));
                _coroutines.Add(_temporaryItem, coroutine);
            }
            else
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

                if (_targetPosition.ContainsKey(itemPosition))
                    _targetPosition.Remove(itemPosition);

                if (_coroutines.ContainsKey(item))
                {
                    StopCoroutine(_coroutines[item]);
                    _coroutines.Remove(item);
                }
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
                    _newMatchedItems[item].Add(currentPosition.Item);
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
            _matchedItems.Clear();
            _checkedPositions.Clear();
            _positions.Clear();
        }
    }
}