using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using ExtensionContent;
using InitializationContent;
using ItemContent;
using ItemPositionContent;
using Road;
using SaveAndLoad;
using SpawnContent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapsContent
{
    public class MapGenerator : MonoBehaviour
    {
        private const string ItemStorageSave = "ItemStorageSave";

        [SerializeField] private Transform[] _environments;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private Territory[] _territorys;
        [SerializeField] private Item[] _items;
        [SerializeField] private RoadGenerator _roadGenerator;
        [SerializeField] private FinderPositions[] _finderPositions;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private ExtensionMap _extensionMap;

        private List<Territory> _targetTerritories = new List<Territory>();
        private List<FinderPositions> _targetFinderPositions = new List<FinderPositions>();
        private List<ItemPosition> _targetItemPositions = new List<ItemPosition>();
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.3f);
        private WaitForSeconds _waitForSecondsMoment = new WaitForSeconds(0.1f);
        private List<ItemPosition> _clearPositions;
        private int _randomIndex;


        public void GenerateMap(int index)
        {
            _targetTerritories = new List<Territory>();
            _targetFinderPositions = new List<FinderPositions>();
            _targetItemPositions = new List<ItemPosition>();

            if (_environments[index].GetComponent<Map>().IsMapExpanding)
            {
                _extensionMap.SearchMap(_environments[index].GetComponent<Map>().Index);
                return;
            }

            Territory[] territories = _environments[index].GetComponentsInChildren<Territory>(true);

            foreach (var territory in territories)
            {
                if (!_targetTerritories.Contains(territory) && !territory.IsExpanding)
                    _targetTerritories.Add(territory);
            }

            FinderPositions[] finderPositionScripts =
                _environments[index].GetComponentsInChildren<FinderPositions>(true);

            foreach (FinderPositions finderPositionScript in finderPositionScripts)
            {
                if (!_targetFinderPositions.Contains(finderPositionScript))
                    _targetFinderPositions.Add(finderPositionScript);
            }

            ItemPosition[] itemPositions = _environments[index].GetComponentsInChildren<ItemPosition>(true);

            foreach (var itemPosition in itemPositions)
            {
                if (!itemPosition.enabled)
                    continue;

                if (!_targetItemPositions.Contains(itemPosition))
                    _targetItemPositions.Add(itemPosition);
            }

            foreach (var territory in _targetTerritories)
                territory.gameObject.SetActive(false);

            FastMapGeneration(_targetTerritories, _targetFinderPositions,
                _environments[index].GetComponent<Map>().RoadsContainer,
                _targetItemPositions, _environments[index].GetComponent<Map>().StartItems,
                _environments[index].GetComponent<Map>());
        }

        /*
        public void Generation()
        {
            foreach (var territory in _territorys)
            {
                territory.gameObject.SetActive(false);
                _roadGenerator.DeactivateRoad();
            }

            StartCoroutine(StartGenerationTerritory());
        }

        private IEnumerator StartGenerationTerritory()
        {
            foreach (var territory in _territorys)
            {
                territory.gameObject.SetActive(true);
                territory.PositionActivation();
                yield return _waitForSeconds;
            }

            yield return null;

            foreach (var finderPosition in _finderPositions)
                finderPosition.FindNeighbor();

            yield return _waitForSecondsMoment;
            _roadGenerator.OnGeneration();
            yield return _waitForSecondsMoment;
            _spawner.OnCreateItem();
            yield return _waitForSeconds;
        }
        */

        public void GenerationWithoutSpawn(List<Territory> territories, List<FinderPositions> finderPositions,
            List<ItemPosition> itemPositions, Transform container, List<Item> startItems)
        {
            foreach (var territory in territories)
                territory.gameObject.SetActive(false);

            StartCoroutine(StartGenerationWithoutSpawn(territories, finderPositions, itemPositions, container,
                startItems));
        }

        private IEnumerator StartGenerationWithoutSpawn(List<Territory> territories,
            List<FinderPositions> finderPositions,
            List<ItemPosition> itemPositions, Transform container, List<Item> startItems)
        {
            foreach (var territory in territories)
            {
                PositionActivation(territory);
                yield return _waitForSeconds;
            }

            yield return null;

            foreach (var item in startItems)
            {
                ArrangeItems(itemPositions, item, _initializator.CurrentMap.ItemsContainer);
                yield return _waitForSecondsMoment;
            }

            foreach (var finderPosition in finderPositions)
                finderPosition.FindNeighbor();

            yield return _waitForSecondsMoment;
            _roadGenerator.TestGeneration(itemPositions, container, _initializator.CurrentMap);
        }

        public void FastMapGeneration(List<Territory> territories, List<FinderPositions> finderPositions,
            Transform container,
            List<ItemPosition> itemPositions, List<Item> startItems, Map map)
        {
            foreach (var territory in territories)
            {
                territory.gameObject.SetActive(true);
                territory.ShowPositions();
            }

            SaveData saveData = new SaveData();

            if (PlayerPrefs.HasKey(ItemStorageSave + map.Index))
            {
                LoadItems(saveData,map);
            }
            else
            {
                foreach (var item in startItems)
                {
                    _clearPositions = new List<ItemPosition>();
                    _clearPositions = itemPositions
                        .Where(p => !p.GetComponent<ItemPosition>().IsBusy && !p.GetComponent<ItemPosition>().IsWater &&
                                    p.GetComponent<ItemPosition>().gameObject.activeSelf)
                        .ToList();
                    _randomIndex = Random.Range(0, _clearPositions.Count);
                    Item newItem = Instantiate(item, map.ItemsContainer);
                    newItem.transform.position = _clearPositions[_randomIndex].transform.position;
                    newItem.Init(_clearPositions[_randomIndex]);
                    newItem.Activation();
                }
            }

            if (!map.IsMapExpanding)
            {
                foreach (var finderPosition in finderPositions)
                {
                    if (finderPosition.gameObject.activeSelf == true)
                        finderPosition.FindNeighbor();
                }
            }

            _roadGenerator.TestGeneration(itemPositions, container, map);
        }


        public void GenerationCurrentMap(List<Territory> territories, List<FinderPositions> finderPositions,
            List<Item> startItems,
            List<ItemPosition> itemPositions, Transform container)
        {
            foreach (var territory in territories)
                territory.gameObject.SetActive(false);

            StartCoroutine(StartGenerationCurrentMap(territories, finderPositions, startItems, itemPositions,
                container));
        }

        private IEnumerator StartGenerationCurrentMap(List<Territory> territories,
            List<FinderPositions> finderPositions,
            List<Item> startItems,
            List<ItemPosition> itemPositions, Transform container)
        {
            foreach (var territory in territories)
            {
                PositionActivation(territory);
                yield return _waitForSeconds;
            }

            yield return null;

            foreach (var item in startItems)
            {
                ArrangeItems(itemPositions, item, container);
                yield return _waitForSecondsMoment;
            }

            if (!_initializator.CurrentMap.IsMapExpanding)
            {
                foreach (var finderPosition in finderPositions)
                    finderPosition.FindNeighbor();
            }

            yield return _waitForSecondsMoment;
            _roadGenerator.TestGeneration(_initializator.ItemPositions, _initializator.CurrentMap.RoadsContainer,
                _initializator.CurrentMap);
            yield return _waitForSecondsMoment;
            _spawner.OnCreateItem();
        }

        private Item GetItem(Items itemName)
        {
            foreach (var item in _items)
            {
                if (item.ItemName == itemName)
                    return item;
            }

            return null;
        }

        /*public void TestVisualGeneration(List<Territory> territories, List<FinderPositions> finderPositions,
            List<Item> startItems,
            List<ItemPosition> itemPositions, Transform container)
        {
            foreach (var territory in territories)
                territory.gameObject.SetActive(false);

            StartCoroutine(StartTestVisualGenerationTerritory(territories, finderPositions, startItems, itemPositions,
                container));
        }

        private IEnumerator StartTestVisualGenerationTerritory(List<Territory> territories,
            List<FinderPositions> finderPositions,
            List<Item> startItems,
            List<ItemPosition> itemPositions, Transform container)
        {
            foreach (var territory in territories)
            {
                PositionActivation(territory);
                yield return _waitForSeconds;
            }

            yield return null;

            foreach (var item in startItems)
            {
                ArrangeItems(itemPositions, item, container);
                yield return null;
            }

            foreach (var finderPosition in finderPositions)
                finderPosition.FindNeighbor();

            yield return _waitForSecondsMoment;
            _roadGenerator.TestGeneration(_initializator.ItemPositions, _initializator.CurrentMap.RoadsContainer,
                _initializator.CurrentMap);
        }*/

        private void PositionActivation(Territory territory)
        {
            territory.gameObject.SetActive(true);
            territory.PositionActivation();
        }

        private void ArrangeItems(List<ItemPosition> itemPositions, Item item, Transform container)
        {
            _clearPositions = new List<ItemPosition>();
            _clearPositions = itemPositions
                .Where(p => !p.GetComponent<ItemPosition>().IsBusy && !p.GetComponent<ItemPosition>().IsWater)
                .ToList();
            _randomIndex = Random.Range(0, _clearPositions.Count);
            Item newItem = Instantiate(item, container);
            newItem.transform.position = _clearPositions[_randomIndex].transform.position;
            newItem.Init(_clearPositions[_randomIndex]);
            newItem.Activation();
        }

        private void LoadItems(SaveData saveData,Map map)
        {
            string jsonData = PlayerPrefs.GetString(ItemStorageSave + map.Index);
            saveData = JsonUtility.FromJson<SaveData>(jsonData);

            foreach (var itemData in saveData.ItemDatas)
            {
                if (itemData != null)
                {
                    if (itemData.ItemName != Items.Crane)
                    {
                        Item item = Instantiate(GetItem(itemData.ItemName),
                            itemData.ItemPosition.transform.position,
                            Quaternion.identity, map.ItemsContainer);
                        item.Init(itemData.ItemPosition);
                        item.Activation();
                    }
                }
            }
        }
    }
}