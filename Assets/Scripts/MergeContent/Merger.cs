using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using InitializationContent;
using ItemContent;
using ItemPositionContent;
using UnityEngine;
using Wallets;

namespace MergeContent
{
    public class Merger : MonoBehaviour
    {
        [SerializeField] private LookMerger _lookMerger;
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Initializator _initializator;
        [SerializeField]private AnimationMatches _animationMatches;

        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
        private WaitForSeconds _pauseBeforeMerge = new WaitForSeconds(0.35f);
        private Item _currentItem;
        private List<ItemPosition> _matchPositions = new List<ItemPosition>();
        private List<Item> _matchItems = new List<Item>();
        private Dictionary<Item, Item> _targetItem = new Dictionary<Item, Item>();

        public event Action<int, int, ItemPosition> Merged;

        public event Action Mergered;

        public event Action<Item> ItemMergered;

        public void MergeItems(ItemPosition currentPosition, List<ItemPosition> matchPositions, List<Item> matchItems, Item targetItem)
        {
            if (!_targetItem.ContainsKey(targetItem))
                _targetItem.Add(targetItem, targetItem);

            _matchPositions = matchPositions;
            _matchItems = matchItems;
            _currentItem = currentPosition.Item;

            foreach (Item item in _matchItems)
                item.Deactivation();

            _animationMatches.StartMoveTarget(_currentItem,currentPosition.transform.position);
            StartCoroutine(MergeActivation(currentPosition, targetItem));
        }

        private IEnumerator MergeActivation(ItemPosition currentPosition, Item targetItem)
        {
            yield return _pauseBeforeMerge;
            ClearPosition();

            if (_currentItem.ItemName == Items.Crane)
                _currentItem = _targetItem[targetItem];

            Item item = Instantiate(
                _targetItem[targetItem].NextItem,
                currentPosition.transform.position,
                Quaternion.identity,
                _initializator.CurrentMap.ItemsContainer);
            ItemActivation(currentPosition, item);
            yield return _waitForSeconds;
            Merged?.Invoke(_matchPositions.Count, _currentItem.Reward, currentPosition);
            _lookMerger.LookAround(currentPosition, item);
            yield return _waitForSeconds;
            Mergered?.Invoke();
            ItemMergered?.Invoke(item);
        }

        private void ItemActivation(ItemPosition currentPosition, Item item)
        {
            if (item.TryGetComponent(out TreasureChest treasureChest))
                treasureChest.Init(_goldWallet);
            item.transform.forward = currentPosition.transform.forward;
            _audioSource.PlayOneShot(_audioSource.clip);
            item.Init(currentPosition);
            item.Activation();
            item.GetComponent<ItemAnimation>().PositioningAnimation();
        }

        private void ClearPosition()
        {
            foreach (var itemPosition in _matchPositions)
            {
                foreach (var position in itemPosition.RoadPositions)
                    position.DisableRoad();

                itemPosition.ClearingPosition();
            }
        }
    }
}