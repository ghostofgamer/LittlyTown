using System;
using System.Collections;
using System.Collections.Generic;
using Dragger;
using ItemContent;
using MergeContent;
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
        private Coroutine _coroutine;

        public event Action CheckCompleted;

        private void OnEnable()
        {
            _itemThrower.BuildItem += AddLightHouse;
            _itemThrower.PlaceChanged += CheckHousesAround;
            _removalItems.ItemRemoved += RemoveLightHouse;
            _replacementPosition.PositionsChanged += CheckHousesAround;
            _merger.Mergered += CheckHousesAround;
        }

        private void OnDisable()
        {
            _itemThrower.BuildItem -= AddLightHouse;
            _itemThrower.PlaceChanged -= CheckHousesAround;
            _removalItems.ItemRemoved -= RemoveLightHouse;
            _replacementPosition.PositionsChanged -= CheckHousesAround;
            _merger.Mergered -= CheckHousesAround;
        }

        private void AddLightHouse(Item item)
        {
            LightHouseTrigger lightHouseTrigger = item.GetComponent<LightHouseTrigger>();

            if (lightHouseTrigger != null)
                _lightHouses.Add(lightHouseTrigger);
        }

        private void RemoveLightHouse(Item item)
        {
            LightHouseTrigger lightHouseTrigger = item.GetComponent<LightHouseTrigger>();

            if (lightHouseTrigger != null)
            {
                lightHouseTrigger.RemoveHouses();
                _lightHouses.Remove(lightHouseTrigger);
            }
        }

        private void CheckHousesAround()
        {
            StartCoroutine(StartLookAround());
        }

        private IEnumerator StartLookAround()
        {
            yield return new WaitForSeconds(0.165f);

            foreach (var lightHouse in _lightHouses)
                lightHouse.LookAround();

            yield return null;
            CheckCompleted?.Invoke();
        }
    }
}