using System;
using System.Collections;
using System.Collections.Generic;
using Dragger;
using ItemContent;
using MergeContent;
using PossibilitiesContent;
using UnityEngine;

namespace Keeper
{
    public class LightHouseKeeper : MonoBehaviour
    {
        [SerializeField] private RemovalItems _removalItems;
        [SerializeField] private ReplacementPosition _replacementPosition;
        [SerializeField] private Merger _merger;
        [SerializeField] private ItemThrower _itemThrower;

        private List<LightHouseTrigger> _lightHouses = new List<LightHouseTrigger>();
        private LightHouseTrigger _lightHouseTrigger;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.165f);

        public event Action CheckCompleted;

        private void OnEnable()
        {
            _itemThrower.BuildItem += OnAddLightHouse;
            _itemThrower.PlaceChanged += OnCheckHousesAround;
            _removalItems.ItemRemoved += OnRemoveLightHouse;
            _replacementPosition.PositionsChanged += OnCheckHousesAround;
            _merger.Mergered += OnCheckHousesAround;
        }

        private void OnDisable()
        {
            _itemThrower.BuildItem -= OnAddLightHouse;
            _itemThrower.PlaceChanged -= OnCheckHousesAround;
            _removalItems.ItemRemoved -= OnRemoveLightHouse;
            _replacementPosition.PositionsChanged -= OnCheckHousesAround;
            _merger.Mergered -= OnCheckHousesAround;
        }

        private void OnAddLightHouse(Item item)
        {
            _lightHouseTrigger = item.GetComponent<LightHouseTrigger>();

            if (_lightHouseTrigger != null)
                _lightHouses.Add(_lightHouseTrigger);
        }

        private void OnRemoveLightHouse(Item item)
        {
            _lightHouseTrigger = item.GetComponent<LightHouseTrigger>();

            if (_lightHouseTrigger != null)
            {
                _lightHouseTrigger.RemoveHouses();
                _lightHouses.Remove(_lightHouseTrigger);
            }
        }

        private void OnCheckHousesAround()
        {
            StartCoroutine(StartLookAround());
        }

        private IEnumerator StartLookAround()
        {
            yield return _waitForSeconds;

            foreach (var lightHouse in _lightHouses)
                lightHouse.LookAround();

            yield return null;
            CheckCompleted?.Invoke();
        }
    }
}