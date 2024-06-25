using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CountersContent;
using InitializationContent;
using ItemContent;
using ItemPositionContent;
using Keeper;
using MergeContent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpawnContent
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Initializator _initializator;
        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private DropGenerator _dropGenerator;
        [SerializeField] private LookMerger _lookMerger;

        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.165f);
        private Coroutine _coroutine;
        private ItemPosition _position;

        public event Action ItemCreated;

        public event Action PositionsFilled;

        public event Action<ItemPosition, Item> AroundLooking;

        private void OnEnable()
        {
            _lookMerger.NotMerged += OnCreateItem;
        }

        private void OnDisable()
        {
            _lookMerger.NotMerged -= OnCreateItem;
        }

        public void OnCreateItem()
        {
            if (!_moveCounter.IsThereMoves || _itemKeeper.SelectedObject != null || _itemKeeper.TemporaryItem != null)
                return;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            StartCoroutine(CreateNewItem());
        }

        public ItemPosition GetPosition()
        {
            List<ItemPosition> freePositions = _initializator.ItemPositions
                .Where(p => !p.GetComponent<ItemPosition>().IsBusy && !p.GetComponent<ItemPosition>().IsWater).ToList();
            int randomIndex = Random.Range(0, freePositions.Count);

            if (freePositions.Count <= 0)
            {
                PositionsFilled?.Invoke();
                return null;
            }

            ItemPosition randomFreePosition = freePositions[randomIndex];
            return randomFreePosition;
        }

        private IEnumerator CreateNewItem()
        {
            yield return _waitForSeconds;
            _position = GetPosition();

            if (_position == null)
                yield break;

            Item item = Instantiate(
                _dropGenerator.GetItem(),
                _position.transform.position,
                Quaternion.identity,
                _initializator.CurrentMap.ItemsContainer);
            _itemKeeper.SetItem(item, _position);
            ItemCreated?.Invoke();
            AroundLooking?.Invoke(_position, item);
        }
    }
}