using System.Collections;
using System.Collections.Generic;
using Dragger;
using ItemContent;
using ItemPositionContent;
using MapsContent;
using Road;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private Initializator _initializator;


    [SerializeField] private Spawner _spawner;
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private Merger _merger;
    [SerializeField] private Transform _container;
    [SerializeField] private RemovalItems _removalItems;
    [SerializeField] private ReplacementPosition _replacementPosition;
    [SerializeField] private ItemDragger _itemDragger;

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

    private Dictionary<string, ItemPosition> _tileConfigurations;
    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);

    private void OnEnable()
    {
        _itemDragger.PlaceChanged += OnGeneration;
        _spawner.ItemCreated += OnGeneration;
        _removalItems.Removed += OnGeneration;
        _replacementPosition.PositionsChanged += OnGeneration;
        _merger.Mergered += OnGeneration;
    }

    private void OnDisable()
    {
        _itemDragger.PlaceChanged -= OnGeneration;
        _spawner.ItemCreated -= OnGeneration;
        _removalItems.Removed -= OnGeneration;
        _replacementPosition.PositionsChanged -= OnGeneration;
        _merger.Mergered -= OnGeneration;
    }

    private void Start()
    {
        _tileConfigurations = new Dictionary<string, ItemPosition>()
        {
            {"0000", _clearTile},
            {"1001", _straightTileHorizontal},
            {"0110", _straightTileVertical},
            {"1111", _fullCrossroadsTile},
            {"1000", _endTileUp},
            {"0100", _endTileLeft},
            {"0010", _endTileRight},
            {"0001", _endTileDown},
            {"1100", _angularTileUpLeft},
            {"1010", _angularTileUpRight},
            {"0011", _angularTileDownRight},
            {"0101", _angularTileDownLeft},
            {"1110", _crossroadsTileUp},
            {"1101", _crossroadsTileLeft},
            {"1011", _crossroadsTileRight},
            {"0111", _crossroadsTileDown},
        };
    }

    public void OnGeneration()
    {
        // Debug.Log("OnGeneration");

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CreateRoad());
    }

    public void DeactivateRoad()
    {
        Transform[] children = _container.GetComponentsInChildren<Transform>(true);

        if (children.Length > 0)
        {
            foreach (Transform child in children)
            {
                if (child != _container.transform && child.GetComponent<RoadTile>())
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private IEnumerator CreateRoad()
    {
        // Debug.Log("Createeeeee");
        yield return _waitForSeconds;

        foreach (ItemPosition itemPosition in _initializator.ItemPositions)
        {
            if (itemPosition.IsWater || itemPosition.IsElevation)
            {
                // Debug.Log("Выключенный Item Position " + itemPosition.name);
                continue;
            }

            if (!itemPosition.IsBusy)
            {
                string surroundingTiles = CheckSurroundingTiles(itemPosition);
                ItemPosition selectedTile = Instantiate(_tileConfigurations[surroundingTiles],
                    itemPosition.transform.position, _initializator.CurrentMap.RoadsContainer.transform.rotation,
                    _initializator.CurrentMap.RoadsContainer);

                itemPosition.SetRoad(selectedTile);
            }

            else
            {
                ItemPosition selectedTile =
                    Instantiate(_clearTile, itemPosition.transform.position,
                        _initializator.CurrentMap.RoadsContainer.transform.rotation,
                        _initializator.CurrentMap.RoadsContainer);

                itemPosition.SetRoad(selectedTile);
            }

            yield return null;
        }
    }

    /*private IEnumerator CreateRoad()
    {
        Debug.Log("Createeeeee");
        yield return _waitForSeconds;

        foreach (ItemPosition itemPosition in _itemPositions)
        {
            if (itemPosition.IsWater || itemPosition.IsElevation)
            {
                continue;
            }

            if (!itemPosition.IsBusy)
            {
                string surroundingTiles = CheckSurroundingTiles(itemPosition);
                ItemPosition selectedTile = Instantiate(_tileConfigurations[surroundingTiles],
                    itemPosition.transform.position, _container.transform.rotation, _container);

                itemPosition.SetRoad(selectedTile);
            }

            else
            {
                ItemPosition selectedTile =
                    Instantiate(_clearTile, itemPosition.transform.position, _container.transform.rotation, _container);

                itemPosition.SetRoad(selectedTile);
            }

            yield return null;
        }
    }*/

    private string CheckSurroundingTiles(ItemPosition itemPosition)
    {
        string surroundingTiles = "0000";

        if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy &&
            itemPosition.NorthPosition.gameObject.activeInHierarchy&&!itemPosition.NorthPosition.IsWater)
        {
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("NORTH " + itemPosition.NorthPosition.name );*/
            surroundingTiles = "1" + surroundingTiles.Substring(1);
        }

        if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy &&
            itemPosition.WestPosition.gameObject.activeInHierarchy&&!itemPosition.WestPosition.IsWater)
        {
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("West " + itemPosition.WestPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 1) + "1" + surroundingTiles.Substring(2);
        }

        if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy &&
            itemPosition.EastPosition.gameObject.activeInHierarchy&&!itemPosition.EastPosition.IsWater)
        {
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("East " + itemPosition.EastPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 2) + "1" + surroundingTiles.Substring(3);
        }

        if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy &&
            itemPosition.SouthPosition.gameObject.activeInHierarchy&&!itemPosition.SouthPosition.IsWater)
        {
            surroundingTiles = surroundingTiles.Substring(0, surroundingTiles.Length - 1) + "1";
        }

        return surroundingTiles;
    }

    public void TestGeneration(List<ItemPosition> itemPositions, Transform container, Map map)
    {
        StartCoroutine(TestCreateRoad(itemPositions, container, map));
    }

    private IEnumerator TestCreateRoad(List<ItemPosition> itemPositions, Transform container, Map map)
    {
        yield return _waitForSeconds;

        foreach (ItemPosition itemPosition in itemPositions)
        {
            if (itemPosition.IsWater || itemPosition.IsElevation)
            {
                continue;
            }

            if (!itemPosition.IsBusy)
            {
                string surroundingTiles = CheckSurroundingTiles(itemPosition);
                ItemPosition selectedTile = Instantiate(_tileConfigurations[surroundingTiles],
                    itemPosition.transform.position, container.transform.rotation, container);

                // Debug.Log("CONTAINER " + container.name);

                itemPosition.SetRoad(selectedTile);
            }

            else
            {
                ItemPosition selectedTile =
                    Instantiate(_clearTile, itemPosition.transform.position, container.transform.rotation, container);

                itemPosition.SetRoad(selectedTile);
            }

            yield return null;
        }

        if (map.Index != _initializator.Index)
        {
            map.gameObject.SetActive(false);
        }
    }

    public void TestCreateRoadOneMoment(List<ItemPosition> itemPositions, Transform container)
    {
        Debug.Log("Moment " + container.name);

        foreach (ItemPosition itemPosition in itemPositions)
        {
            if (itemPosition.IsWater || itemPosition.IsElevation)
            {
                continue;
            }

            if (!itemPosition.IsBusy)
            {
                Debug.Log("Не бизи  " + itemPosition.name);
                string surroundingTiles = CheckSurroundingTiles(itemPosition);
                ItemPosition selectedTile = Instantiate(_tileConfigurations[surroundingTiles],
                    itemPosition.transform.position, container.transform.rotation, container);

                itemPosition.SetRoad(selectedTile);
            }

            else
            {
                Debug.Log("бизи  " + itemPosition.name);
                ItemPosition selectedTile =
                    Instantiate(_clearTile, itemPosition.transform.position, container.transform.rotation, container);

                itemPosition.SetRoad(selectedTile);
            }
        }
    }
}