using System.Collections;
using System.Collections.Generic;
using Dragger;
using InitializationContent;
using ItemContent;
using ItemPositionContent;
using MapsContent;
using MergeContent;
using PossibilitiesContent;
using SpawnContent;
using UnityEngine;

namespace Road
{
    public class RoadGenerator : MonoBehaviour
    {
        private const string ClearTile = "0000";
        private const string RoadCode = "3333";
        private const string RoadCodeNumber = "2";
        private const string TrailCodeNumber = "1";

        [SerializeField] private Initializator _initializator;
        [SerializeField] private Spawner _spawner;
        [SerializeField] private Merger _merger;
        [SerializeField] private RemovalItems _removalItems;
        [SerializeField] private ReplacementPosition _replacementPosition;
        [Header("Tiles")] [SerializeField] private ItemPosition _angularTileUpRight;
        [SerializeField] private ItemPosition _angularTileUpLeft;
        [SerializeField] private ItemPosition _angularTileDownLeft;
        [SerializeField] private ItemPosition _angularTileDownRight;
        [SerializeField] private ItemPosition _endTileUp;
        [SerializeField] private ItemPosition _endTileLeft;
        [SerializeField] private ItemPosition _endTileRight;
        [SerializeField] private ItemPosition _endTileDown;
        [SerializeField] private ItemPosition _fullCrossroadsTile;
        [SerializeField] private ItemPosition _clearTile;
        [SerializeField] private ItemPosition _straightTileHorizontal;
        [SerializeField] private ItemPosition _straightTileVertical;
        [SerializeField] private ItemPosition _crossroadsTileUp;
        [SerializeField] private ItemPosition _crossroadsTileLeft;
        [SerializeField] private ItemPosition _crossroadsTileRight;
        [SerializeField] private ItemPosition _crossroadsTileDown;
        [SerializeField] private ItemPosition _roadTileGrey;
        [SerializeField] private ItemPosition _roadTileUpRight;
        [SerializeField] private ItemPosition _roadTileUpLeft;
        [SerializeField] private ItemPosition _roadTileDownLeft;
        [SerializeField] private ItemPosition _roadTileDownRight;
        [SerializeField] private ItemPosition _roadTileHorizontal;
        [SerializeField] private ItemPosition _roadTileVertical;
        [SerializeField] private ItemPosition _endRoadTileUp;
        [SerializeField] private ItemPosition _endRoadTileLeft;
        [SerializeField] private ItemPosition _endRoadTileRight;
        [SerializeField] private ItemPosition _endRoadTileDown;
        [SerializeField] private ItemPosition _fullRoadTile;
        [SerializeField] private ItemPosition _crossroadsRoadTileUp;
        [SerializeField] private ItemPosition _crossroadsRoadTileLeft;
        [SerializeField] private ItemPosition _crossroadsRoadTileRight;
        [SerializeField] private ItemPosition _crossroadsRoadTileDown;
        [SerializeField] private ItemThrower _itemThrower;

        private Dictionary<string, ItemPosition> _tileConfigurations;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
        private int _minAmountRoads = 1;
        private int _firstIndex = 1;
        private int _secondIndex = 2;
        private int _thirdIndex = 3;
        private int _zero = 0;

        private void OnEnable()
        {
            _itemThrower.PlaceChanged += OnGeneration;
            _spawner.ItemCreated += OnGeneration;
            _removalItems.Removed += OnGeneration;
            _replacementPosition.PositionsChanged += OnGeneration;
            _merger.Mergered += OnGeneration;
            _removalItems.ItemRemoved += OnClearRoad;
            _replacementPosition.PositionItemsChanging += OnClearRoad;
        }

        private void OnDisable()
        {
            _itemThrower.PlaceChanged -= OnGeneration;
            _spawner.ItemCreated -= OnGeneration;
            _removalItems.Removed -= OnGeneration;
            _replacementPosition.PositionsChanged -= OnGeneration;
            _merger.Mergered -= OnGeneration;
            _removalItems.ItemRemoved -= OnClearRoad;
            _replacementPosition.PositionItemsChanging -= OnClearRoad;
        }

        private void Start()
        {
            _tileConfigurations = new Dictionary<string, ItemPosition>()
            {
                { "0000", _clearTile },
                { "1001", _straightTileHorizontal },
                { "0110", _straightTileVertical },
                { "1111", _fullCrossroadsTile },
                { "1000", _endTileUp },
                { "0100", _endTileLeft },
                { "0010", _endTileRight },
                { "0001", _endTileDown },
                { "1100", _angularTileUpLeft },
                { "1010", _angularTileUpRight },
                { "0011", _angularTileDownRight },
                { "0101", _angularTileDownLeft },
                { "1110", _crossroadsTileUp },
                { "1101", _crossroadsTileLeft },
                { "1011", _crossroadsTileRight },
                { "0111", _crossroadsTileDown },
                { "2332", _roadTileHorizontal },
                { "3223", _roadTileVertical },
                { "2233", _roadTileUpLeft },
                { "2323", _roadTileUpRight },
                { "3322", _roadTileDownRight },
                { "3232", _roadTileDownLeft },
                { "3333", _roadTileGrey },
                { "2333", _endRoadTileDown },
                { "3233", _endRoadTileRight },
                { "3323", _endRoadTileLeft },
                { "3332", _endRoadTileUp },
                { "2222", _fullRoadTile },
                { "2223", _crossroadsRoadTileUp },
                { "2232", _crossroadsRoadTileLeft },
                { "2322", _crossroadsRoadTileRight },
                { "3222", _crossroadsRoadTileDown },
            };
        }

        public void OnGeneration()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(CreateRoad());
        }

        public void GenerationRoad(List<ItemPosition> itemPositions, Transform container, Map map)
        {
            StartCoroutine(CreateRoadAnotherMap(itemPositions, container, map));
        }

        public void GenerateSandBoxTrail(ItemPosition[] itemPositions, Transform container)
        {
            List<ItemPosition> positionsTrail = new List<ItemPosition>();

            foreach (var itemPosition in itemPositions)
            {
                if (itemPosition.IsTrail)
                    positionsTrail.Add(itemPosition);
            }

            List<ItemPosition> positionsRoad = new List<ItemPosition>();

            foreach (var itemPosition in itemPositions)
            {
                if (itemPosition.IsRoad)
                    positionsRoad.Add(itemPosition);
            }

            foreach (var position in positionsTrail)
            {
                string trail = CheckSurroundingTilesSandBox(position);
                ItemPosition selectedTile =
                    Instantiate(_tileConfigurations[trail], position.transform.position, container.rotation, container);
                position.SetRoad(selectedTile);
            }

            foreach (var position in positionsRoad)
            {
                string trail = CheckSurroundingTilesRoadsSandBox(position);
                ItemPosition selectedTile =
                    Instantiate(_tileConfigurations[trail], position.transform.position, container.rotation, container);

                position.SetRoad(selectedTile);
            }
        }

        private IEnumerator CreateRoad()
        {
            yield return _waitForSeconds;
            List<ItemPosition> positions = new List<ItemPosition>();

            foreach (ItemPosition itemPosition in _initializator.ItemPositions)
            {
                if (itemPosition.IsWater || itemPosition.IsElevation)
                    continue;

                if (itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
                {
                    ItemPosition tile =
                        Instantiate(
                            _roadTileGrey,
                            itemPosition.transform.position,
                            _initializator.CurrentMap.RoadsContainer.transform.rotation,
                            _initializator.CurrentMap.RoadsContainer);

                    itemPosition.SetRoad(tile);

                    foreach (var roadPosition in itemPosition.RoadPositions)
                    {
                        if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                            roadPosition.gameObject.activeInHierarchy)
                            positions.Add(roadPosition);
                    }
                }

                foreach (var roadPosition in positions)
                    roadPosition.EnableRoad();

                foreach (var roadPosition in positions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                        roadPosition.gameObject.activeInHierarchy)
                    {
                        string surroundingTiles = CheckSurroundingRoadTiles(roadPosition);
                        ItemPosition selectedTile =
                            Instantiate(
                                _tileConfigurations[surroundingTiles],
                                roadPosition.transform.position,
                                _initializator.CurrentMap.RoadsContainer.transform.rotation,
                                _initializator.CurrentMap.RoadsContainer);

                        roadPosition.SetRoad(selectedTile);
                    }
                }
            }

            foreach (ItemPosition itemPosition in _initializator.ItemPositions)
            {
                if (itemPosition.IsWater || itemPosition.IsElevation)
                    continue;

                if (!itemPosition.IsBusy && !itemPosition.IsRoad)
                {
                    string surroundingTiles = CheckSurroundingTiles(itemPosition);
                    ItemPosition selectedTile = Instantiate(
                        _tileConfigurations[surroundingTiles],
                        itemPosition.transform.position,
                        _initializator.CurrentMap.RoadsContainer.transform.rotation,
                        _initializator.CurrentMap.RoadsContainer);

                    itemPosition.SetRoad(selectedTile);
                }
                else
                {
                    if (itemPosition.Item != null && itemPosition.Item.IsBigHouse)
                        continue;

                    if (itemPosition.IsRoad)
                        continue;

                    ItemPosition selectedTile =
                        Instantiate(
                            _clearTile,
                            itemPosition.transform.position,
                            _initializator.CurrentMap.RoadsContainer.transform.rotation,
                            _initializator.CurrentMap.RoadsContainer);

                    itemPosition.SetRoad(selectedTile);
                }

                yield return null;
            }
        }

        private string CheckSurroundingTiles(ItemPosition itemPosition)
        {
            string surroundingTiles = ClearTile;

            if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy &&
                itemPosition.NorthPosition.gameObject.activeInHierarchy && !itemPosition.NorthPosition.IsWater &&
                !itemPosition.NorthPosition.IsRoad)
            {
                surroundingTiles = TrailCodeNumber + surroundingTiles.Substring(_firstIndex);
            }

            if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy &&
                itemPosition.WestPosition.gameObject.activeInHierarchy && !itemPosition.WestPosition.IsWater &&
                !itemPosition.WestPosition.IsRoad)
            {
                surroundingTiles = surroundingTiles.Substring(_zero, _firstIndex) + TrailCodeNumber + surroundingTiles.Substring(_secondIndex);
            }

            if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy &&
                itemPosition.EastPosition.gameObject.activeInHierarchy && !itemPosition.EastPosition.IsWater &&
                !itemPosition.EastPosition.IsRoad)
            {
                surroundingTiles = surroundingTiles.Substring(_zero, _secondIndex) + TrailCodeNumber + surroundingTiles.Substring(_thirdIndex);
            }

            if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy &&
                itemPosition.SouthPosition.gameObject.activeInHierarchy && !itemPosition.SouthPosition.IsWater &&
                !itemPosition.SouthPosition.IsRoad)
            {
                surroundingTiles = surroundingTiles.Substring(_zero, surroundingTiles.Length - _firstIndex) + TrailCodeNumber;
            }

            return surroundingTiles;
        }

        private string CheckSurroundingRoadTiles(ItemPosition itemPosition)
        {
            string surroundingTiles = RoadCode;

            if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy &&
                itemPosition.NorthPosition.gameObject.activeInHierarchy && !itemPosition.NorthPosition.IsWater &&
                itemPosition.NorthPosition.IsRoad)
            {
                surroundingTiles = RoadCodeNumber + surroundingTiles.Substring(_firstIndex);
            }

            if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy &&
                itemPosition.WestPosition.gameObject.activeInHierarchy && !itemPosition.WestPosition.IsWater &&
                itemPosition.WestPosition.IsRoad)
            {
                surroundingTiles = surroundingTiles.Substring(_zero, _firstIndex) + RoadCodeNumber + surroundingTiles.Substring(_secondIndex);
            }

            if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy &&
                itemPosition.EastPosition.gameObject.activeInHierarchy && !itemPosition.EastPosition.IsWater &&
                itemPosition.EastPosition.IsRoad)
            {
                surroundingTiles = surroundingTiles.Substring(_zero, _secondIndex) + RoadCodeNumber + surroundingTiles.Substring(_thirdIndex);
            }

            if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy &&
                itemPosition.SouthPosition.gameObject.activeInHierarchy && !itemPosition.SouthPosition.IsWater &&
                itemPosition.SouthPosition.IsRoad)
            {
                surroundingTiles = surroundingTiles.Substring(_zero, surroundingTiles.Length - _firstIndex) + RoadCodeNumber;
            }

            return surroundingTiles;
        }

        private IEnumerator CreateRoadAnotherMap(List<ItemPosition> itemPositions, Transform container, Map map)
        {
            yield return _waitForSeconds;
            List<ItemPosition> positions = new List<ItemPosition>();

            foreach (ItemPosition itemPosition in itemPositions)
            {
                if (itemPosition.IsWater || itemPosition.IsElevation)
                    continue;

                if (itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
                {
                    ItemPosition tile =
                        Instantiate(
                            _roadTileGrey,
                            itemPosition.transform.position,
                            container.transform.rotation,
                            container);

                    itemPosition.SetRoad(tile);

                    foreach (var roadPosition in itemPosition.RoadPositions)
                    {
                        if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                            roadPosition.gameObject.activeInHierarchy)
                            positions.Add(roadPosition);
                    }
                }

                foreach (var roadPosition in positions)
                    roadPosition.EnableRoad();

                foreach (var roadPosition in positions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                        roadPosition.gameObject.activeInHierarchy)
                    {
                        string surroundingTiles = CheckSurroundingRoadTiles(roadPosition);
                        ItemPosition selectedTile =
                            Instantiate(
                                _tileConfigurations[surroundingTiles],
                                roadPosition.transform.position,
                                container.transform.rotation,
                                container);

                        roadPosition.SetRoad(selectedTile);
                    }
                }
            }

            foreach (ItemPosition itemPosition in itemPositions)
            {
                if (itemPosition.IsWater || itemPosition.IsElevation)
                    continue;

                if (!itemPosition.IsBusy && !itemPosition.IsRoad)
                {
                    string surroundingTiles = CheckSurroundingTiles(itemPosition);
                    ItemPosition selectedTile = Instantiate(
                        _tileConfigurations[surroundingTiles],
                        itemPosition.transform.position,
                        container.transform.rotation,
                        container);
                    itemPosition.SetRoad(selectedTile);
                }
                else
                {
                    if (itemPosition.Item != null && itemPosition.Item.IsBigHouse)
                        continue;

                    if (itemPosition.IsRoad)
                        continue;

                    ItemPosition selectedTile = Instantiate(
                        _clearTile,
                        itemPosition.transform.position,
                        container.transform.rotation,
                        container);
                    itemPosition.SetRoad(selectedTile);
                }

                yield return null;
            }

            if (map.Index != _initializator.Index)
                map.gameObject.SetActive(false);
        }

        private void OnClearRoad(Item item)
        {
            foreach (var road in item.ItemPosition.RoadPositions)
                road.DisableRoad();
        }

        private void OnClearRoad(ItemPosition firstItemPositionItem, ItemPosition secondItemPositionItem)
        {
            foreach (var itemPosition in firstItemPositionItem.RoadPositions)
                itemPosition.DisableRoad();

            foreach (var itemPosition in secondItemPositionItem.RoadPositions)
                itemPosition.DisableRoad();
        }

        private string CheckSurroundingTilesSandBox(ItemPosition itemPosition)
        {
            float value = 0;

            string surroundingTiles = ClearTile;

            if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy &&
                itemPosition.NorthPosition.gameObject.activeInHierarchy && !itemPosition.NorthPosition.IsWater &&
                !itemPosition.NorthPosition.IsRoad && itemPosition.NorthPosition.IsTrail)
            {
                value++;
                surroundingTiles = TrailCodeNumber + surroundingTiles.Substring(_firstIndex);
            }

            if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy &&
                itemPosition.WestPosition.gameObject.activeInHierarchy && !itemPosition.WestPosition.IsWater &&
                !itemPosition.WestPosition.IsRoad && itemPosition.WestPosition.IsTrail)
            {
                value++;
                surroundingTiles = surroundingTiles.Substring(_zero, _firstIndex) + TrailCodeNumber + surroundingTiles.Substring(_secondIndex);
            }

            if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy &&
                itemPosition.EastPosition.gameObject.activeInHierarchy && !itemPosition.EastPosition.IsWater &&
                !itemPosition.EastPosition.IsRoad && itemPosition.EastPosition.IsTrail)
            {
                value++;
                surroundingTiles = surroundingTiles.Substring(_zero, _secondIndex) + TrailCodeNumber + surroundingTiles.Substring(_thirdIndex);
            }

            if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy &&
                itemPosition.SouthPosition.gameObject.activeInHierarchy && !itemPosition.SouthPosition.IsWater &&
                !itemPosition.SouthPosition.IsRoad && itemPosition.SouthPosition.IsTrail)
            {
                value++;
                surroundingTiles = surroundingTiles.Substring(_zero, surroundingTiles.Length - _firstIndex) + TrailCodeNumber;
            }

            if (value < _minAmountRoads)
            {
                return ClearTile;
            }

            return surroundingTiles;
        }

        private string CheckSurroundingTilesRoadsSandBox(ItemPosition itemPosition)
        {
            string surroundingTiles = RoadCode;
            int amount = 0;

            if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy &&
                itemPosition.NorthPosition.gameObject.activeInHierarchy && !itemPosition.NorthPosition.IsWater &&
                itemPosition.NorthPosition.IsRoad)
            {
                amount++;
                surroundingTiles = RoadCodeNumber + surroundingTiles.Substring(_firstIndex);
            }

            if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy &&
                itemPosition.WestPosition.gameObject.activeInHierarchy && !itemPosition.WestPosition.IsWater &&
                itemPosition.WestPosition.IsRoad)
            {
                amount++;
                surroundingTiles = surroundingTiles.Substring(_zero, _firstIndex) + RoadCodeNumber + surroundingTiles.Substring(_secondIndex);
            }

            if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy &&
                itemPosition.EastPosition.gameObject.activeInHierarchy && !itemPosition.EastPosition.IsWater &&
                itemPosition.EastPosition.IsRoad)
            {
                amount++;
                surroundingTiles = surroundingTiles.Substring(_zero, _secondIndex) + RoadCodeNumber + surroundingTiles.Substring(_thirdIndex);
            }

            if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy &&
                itemPosition.SouthPosition.gameObject.activeInHierarchy && !itemPosition.SouthPosition.IsWater &&
                itemPosition.SouthPosition.IsRoad)
            {
                amount++;
                surroundingTiles = surroundingTiles.Substring(_zero, surroundingTiles.Length - _firstIndex) + RoadCodeNumber;
            }

            if (amount < _minAmountRoads)
            {
                return ClearTile;
            }

            return surroundingTiles;
        }
    }
}