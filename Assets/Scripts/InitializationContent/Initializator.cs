using System;
using System.Collections.Generic;
using ExtensionContent;
using ItemPositionContent;
using MapsContent;
using SaveAndLoad;
using UnityEngine;

namespace InitializationContent
{
    public class Initializator : MonoBehaviour
    {
        private const string ExtensionTerritory = "ExtensionTerritory";

        [SerializeField] private Transform[] _environments;
        [SerializeField] private ChooseMap _chooseMap;
        [SerializeField] private Load _load;
        [SerializeField] private Save _save;
        [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
        [SerializeField] private ExtensionMap _extensionMap;

        private List<Territory> _extensionFilterTerritories = new List<Territory>();
        private int _index;
        private bool _initExtension;

        public event Action IndexChanged;

        public int Index => _index;

        public int AmountMaps { get; private set; }

        public List<FinderPositions> FinderPositions { get; private set; } = new List<FinderPositions>();

        public List<Territory> Territories { get; private set; } = new List<Territory>();

        public List<ItemPosition> ItemPositions { get; private set; } = new List<ItemPosition>();

        public Map CurrentMap { get; private set; }

        public Transform[] Environments => _environments;

        private void Awake()
        {
            AmountMaps = _environments.Length;
        }

        private void OnEnable()
        {
            _chooseMap.MapChanged += OnSetIndex;
        }

        private void OnDisable()
        {
            _chooseMap.MapChanged -= OnSetIndex;
        }

        public void OnSetIndex(int index)
        {
            _index = index;
            IndexChanged?.Invoke();
        }

        public void ResetIndex()
        {
            _index = 0;
        }

        public void FillLists()
        {
            if (_environments[_index].GetComponent<Map>().IsMapExpanding)
            {
                ExtensionFillLists();
                return;
            }

            CreateNewLists();
            CurrentMap = _environments[_index].GetComponent<Map>();
            Territory[] territories = _environments[_index].GetComponentsInChildren<Territory>(true);
            FinderPositions[] finderPositionScripts =
                _environments[_index].GetComponentsInChildren<FinderPositions>(true);
            ItemPosition[] itemPositions = _environments[_index].GetComponentsInChildren<ItemPosition>(true);

            foreach (var territory in territories)
            {
                if (!Territories.Contains(territory))
                    Territories.Add(territory);
            }

            foreach (FinderPositions finderPositionScript in finderPositionScripts)
            {
                if (!FinderPositions.Contains(finderPositionScript))
                    FinderPositions.Add(finderPositionScript);
            }

            foreach (var itemPosition in itemPositions)
            {
                if (!itemPosition.enabled)
                    continue;

                if (!ItemPositions.Contains(itemPosition))
                    ItemPositions.Add(itemPosition);
            }
        }

        public void SetPositions(List<ItemPosition> positions)
        {
            ItemPositions = positions;
            _visualItemsDeactivator.SetPositions(ItemPositions);
        }

        public void ResetTerritory()
        {
            ItemPositions = new List<ItemPosition>();

            Territory[] territories = _environments[_index].GetComponentsInChildren<Territory>(true);

            foreach (var territory in territories)
            {
                if (territory.IsExpanding)
                {
                    territory.DisableOpened();
                    territory.gameObject.SetActive(false);
                }
            }

            _save.SetData(ExtensionTerritory + _environments[_index].GetComponent<Map>().Index, 0);

            if (_environments[_index].GetComponent<Map>().IsWaterRandom)
                _extensionMap.RandomWater(_environments[_index].GetComponent<Map>());

            _initExtension = true;
            ExtensionFillLists();
        }

        private void CreateNewLists()
        {
            ItemPositions = new List<ItemPosition>();
            Territories = new List<Territory>();
            FinderPositions = new List<FinderPositions>();
        }

        private void ExtensionFillLists()
        {
            CreateNewLists();
            _extensionFilterTerritories = new List<Territory>();
            CurrentMap = _environments[_index].GetComponent<Map>();
            Territory[] territories = _environments[_index].GetComponentsInChildren<Territory>(true);

            foreach (var territory in territories)
            {
                if (territory.IsExpanding)
                    _extensionFilterTerritories.Add(territory);
            }

            int amount = _load.Get(ExtensionTerritory + CurrentMap.Index, 0);

            for (int i = 0; i < amount; i++)
                _extensionFilterTerritories[i].EnableOpened();

            foreach (var territory in territories)
            {
                if (territory.IsExpanding && !territory.IsOpened)
                    continue;

                if ((!Territories.Contains(territory) && !territory.IsExpanding) ||
                    (!Territories.Contains(territory) && territory.IsOpened))
                    Territories.Add(territory);
            }

            foreach (Territory territory in Territories)
            {
                FinderPositions[] finderPositionsInTerritory =
                    territory.gameObject.GetComponentsInChildren<FinderPositions>(true);

                foreach (FinderPositions finder in finderPositionsInTerritory)
                {
                    if (!FinderPositions.Contains(finder))
                        FinderPositions.Add(finder);
                }
            }

            foreach (Territory territory in Territories)
            {
                ItemPosition[] itemPositions = territory.gameObject.GetComponentsInChildren<ItemPosition>(true);

                foreach (ItemPosition itemPosition in itemPositions)
                {
                    if (!itemPosition.enabled)
                        continue;

                    if (!ItemPositions.Contains(itemPosition))
                        ItemPositions.Add(itemPosition);
                }
            }

            if (_environments[_index].GetComponent<Map>().IsMapExpanding)
            {
                _extensionMap.SetMap(_environments[_index].GetComponent<Map>());
                _extensionMap.ContinueMap(
                    Territories,
                    ItemPositions,
                    FinderPositions,
                    _extensionFilterTerritories);
            }

            if (_initExtension)
            {
                _extensionMap.ResetMap(
                    Territories,
                    ItemPositions,
                    FinderPositions,
                    _extensionFilterTerritories);
                _initExtension = false;
            }
        }
    }
}