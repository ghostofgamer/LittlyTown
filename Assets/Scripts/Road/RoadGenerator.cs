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

    private void OnEnable()
    {
        // _itemDragger.PlaceChanged += OnGeneration;
        _itemThrower.PlaceChanged += OnGeneration;
        _spawner.ItemCreated += OnGeneration;
        _removalItems.Removed += OnGeneration;
        _replacementPosition.PositionsChanged += OnGeneration;
        _merger.Mergered += OnGeneration;
        _removalItems.ItemRemoved += ClearRoad;
        _replacementPosition.PositionItemsChanging += ClearRoad;
    }

    private void OnDisable()
    {
        // _itemDragger.PlaceChanged -= OnGeneration;
        _itemThrower.PlaceChanged -= OnGeneration;
        _spawner.ItemCreated -= OnGeneration;
        _removalItems.Removed -= OnGeneration;
        _replacementPosition.PositionsChanged -= OnGeneration;
        _merger.Mergered -= OnGeneration;
        _removalItems.ItemRemoved -= ClearRoad;
        _replacementPosition.PositionItemsChanging -= ClearRoad;
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
            {"2332", _roadTileHorizontal},
            {"3223", _roadTileVertical},
            {"2233", _roadTileUpLeft},
            {"2323", _roadTileUpRight},
            {"3322", _roadTileDownRight},
            {"3232", _roadTileDownLeft},
            {"3333", _roadTileGrey},
            {"2333", _endRoadTileDown},
            {"3233", _endRoadTileRight},
            {"3323", _endRoadTileLeft},
            {"3332", _endRoadTileUp},
            {"2222", _fullRoadTile},
            {"2223", _crossroadsRoadTileUp},
            {"2232", _crossroadsRoadTileLeft},
            {"2322", _crossroadsRoadTileRight},
            {"3222", _crossroadsRoadTileDown},
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

        List<ItemPosition> positions = new List<ItemPosition>();

        foreach (ItemPosition itemPosition in _initializator.ItemPositions)
        {
            /*if (itemPosition.IsElevation && itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
            {
                ItemPosition tile =
                    Instantiate(_roadTileGrey, itemPosition.transform.position,
                        _initializator.CurrentMap.RoadsContainer.transform.rotation,
                        _initializator.CurrentMap.RoadsContainer);

                itemPosition.SetRoad(tile);
            }*/

            if (itemPosition.IsWater || itemPosition.IsElevation)
                continue;

            if (itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
            {
                ItemPosition tile =
                    Instantiate(_roadTileGrey, itemPosition.transform.position,
                        _initializator.CurrentMap.RoadsContainer.transform.rotation,
                        _initializator.CurrentMap.RoadsContainer);

                itemPosition.SetRoad(tile);

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                        roadPosition.gameObject.activeInHierarchy)
                    {
                        positions.Add(roadPosition);
                    }
                }
            }

            foreach (var roadPosition in positions)
            {
                roadPosition.EnableRoad();
            }

            foreach (var roadPosition in positions)
            {
                if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                    roadPosition.gameObject.activeInHierarchy)
                {
                    string surroundingTiles = CheckSurroundingRoadTiles(roadPosition);
                    ItemPosition selectedTile =
                        Instantiate(_tileConfigurations[surroundingTiles], roadPosition.transform.position,
                            _initializator.CurrentMap.RoadsContainer.transform.rotation,
                            _initializator.CurrentMap.RoadsContainer);

                    roadPosition.SetRoad(selectedTile);
                }
            }
        }


        /*foreach (ItemPosition itemPosition in _initializator.ItemPositions)
        {
            if (itemPosition.IsWater || itemPosition.IsElevation)
            {
                // Debug.Log("Выключенный Item Position " + itemPosition.name);
                continue;
            }

            if (itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
            {
                ItemPosition tile =
                    Instantiate(_roadTileGrey, itemPosition.transform.position,
                        _initializator.CurrentMap.RoadsContainer.transform.rotation,
                        _initializator.CurrentMap.RoadsContainer);

                itemPosition.SetRoad(tile);
                /*itemPosition.EnableRoad();#1#

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                        roadPosition.gameObject.activeInHierarchy)
                    {
                        roadPosition.EnableRoad();
                    }
                }

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                        roadPosition.gameObject.activeInHierarchy)
                    {
                        string surroundingTiles = CheckSurroundingRoadTiles(roadPosition);
                        ItemPosition selectedTile =
                            Instantiate(_tileConfigurations[surroundingTiles], roadPosition.transform.position,
                                _initializator.CurrentMap.RoadsContainer.transform.rotation,
                                _initializator.CurrentMap.RoadsContainer);

                        roadPosition.SetRoad(selectedTile);
                    }
                }
            }
        }*/


        foreach (ItemPosition itemPosition in _initializator.ItemPositions)
        {
            if (itemPosition.IsWater || itemPosition.IsElevation)
            {
                // Debug.Log("Выключенный Item Position " + itemPosition.name);
                continue;
            }

            /*if (itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
            {
                ItemPosition tile =
                    Instantiate(_roadTileGrey, itemPosition.transform.position,
                        _initializator.CurrentMap.RoadsContainer.transform.rotation,
                        _initializator.CurrentMap.RoadsContainer);

                itemPosition.SetRoad(tile);
                /*itemPosition.EnableRoad();#1#

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy&&!roadPosition.IsWater&&roadPosition.gameObject.activeInHierarchy)
                    {
                        roadPosition.EnableRoad();
                    }
                }

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy&&!roadPosition.IsWater&&roadPosition.gameObject.activeInHierarchy)
                    {
                        string surroundingTiles = CheckSurroundingRoadTiles(roadPosition);
                        ItemPosition selectedTile =
                            Instantiate(_tileConfigurations[surroundingTiles], roadPosition.transform.position,
                                _initializator.CurrentMap.RoadsContainer.transform.rotation,
                                _initializator.CurrentMap.RoadsContainer);

                        roadPosition.SetRoad(selectedTile);
                    }
                }
            }*/

            if (!itemPosition.IsBusy && !itemPosition.IsRoad)
            {
                string surroundingTiles = CheckSurroundingTiles(itemPosition);
                ItemPosition selectedTile = Instantiate(_tileConfigurations[surroundingTiles],
                    itemPosition.transform.position, _initializator.CurrentMap.RoadsContainer.transform.rotation,
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
            itemPosition.NorthPosition.gameObject.activeInHierarchy && !itemPosition.NorthPosition.IsWater &&
            !itemPosition.NorthPosition.IsRoad)
        {
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("NORTH " + itemPosition.NorthPosition.name );*/
            surroundingTiles = "1" + surroundingTiles.Substring(1);
        }

        if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy &&
            itemPosition.WestPosition.gameObject.activeInHierarchy && !itemPosition.WestPosition.IsWater &&
            !itemPosition.WestPosition.IsRoad)
        {
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("West " + itemPosition.WestPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 1) + "1" + surroundingTiles.Substring(2);
        }

        if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy &&
            itemPosition.EastPosition.gameObject.activeInHierarchy && !itemPosition.EastPosition.IsWater &&
            !itemPosition.EastPosition.IsRoad)
        {
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("East " + itemPosition.EastPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 2) + "1" + surroundingTiles.Substring(3);
        }

        if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy &&
            itemPosition.SouthPosition.gameObject.activeInHierarchy && !itemPosition.SouthPosition.IsWater &&
            !itemPosition.SouthPosition.IsRoad)
        {
            surroundingTiles = surroundingTiles.Substring(0, surroundingTiles.Length - 1) + "1";
        }

        return surroundingTiles;
    }

    private string CheckSurroundingRoadTiles(ItemPosition itemPosition)
    {
        string surroundingTiles = "3333";
        int amount = 0;

        if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy &&
            itemPosition.NorthPosition.gameObject.activeInHierarchy && !itemPosition.NorthPosition.IsWater &&
            itemPosition.NorthPosition.IsRoad)
        {
            amount++;
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("NORTH " + itemPosition.NorthPosition.name );*/
            surroundingTiles = "2" + surroundingTiles.Substring(1);
        }

        if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy &&
            itemPosition.WestPosition.gameObject.activeInHierarchy && !itemPosition.WestPosition.IsWater &&
            itemPosition.WestPosition.IsRoad)
        {
            amount++;
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("West " + itemPosition.WestPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 1) + "2" + surroundingTiles.Substring(2);
        }

        if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy &&
            itemPosition.EastPosition.gameObject.activeInHierarchy && !itemPosition.EastPosition.IsWater &&
            itemPosition.EastPosition.IsRoad)
        {
            amount++;
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("East " + itemPosition.EastPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 2) + "2" + surroundingTiles.Substring(3);
        }

        if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy &&
            itemPosition.SouthPosition.gameObject.activeInHierarchy && !itemPosition.SouthPosition.IsWater &&
            itemPosition.SouthPosition.IsRoad)
        {
            amount++;
            surroundingTiles = surroundingTiles.Substring(0, surroundingTiles.Length - 1) + "2";
        }

        /*if (amount < 1||amount>2)
        {
            return "2222";
        }*/

        return surroundingTiles;
    }

    public void TestGeneration(List<ItemPosition> itemPositions, Transform container, Map map)
    {
        StartCoroutine(TestCreateRoad(itemPositions, container, map));
    }

    private IEnumerator TestCreateRoad(List<ItemPosition> itemPositions, Transform container, Map map)
    {
        yield return _waitForSeconds;

        List<ItemPosition> positions = new List<ItemPosition>();

        foreach (ItemPosition itemPosition in itemPositions)
        {
            /*if (itemPosition.IsElevation && itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
            {
                ItemPosition tile =
                    Instantiate(_roadTileGrey, itemPosition.transform.position,
                        _initializator.CurrentMap.RoadsContainer.transform.rotation,
                        _initializator.CurrentMap.RoadsContainer);

                itemPosition.SetRoad(tile);
            }*/

            if (itemPosition.IsWater || itemPosition.IsElevation)
                continue;

            if (itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
            {
                ItemPosition tile =
                    Instantiate(_roadTileGrey, itemPosition.transform.position,
                        container.transform.rotation,
                        container);

                itemPosition.SetRoad(tile);

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                        roadPosition.gameObject.activeInHierarchy)
                    {
                        positions.Add(roadPosition);
                    }
                }
            }

            foreach (var roadPosition in positions)
            {
                roadPosition.EnableRoad();
            }

            foreach (var roadPosition in positions)
            {
                if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                    roadPosition.gameObject.activeInHierarchy)
                {
                    string surroundingTiles = CheckSurroundingRoadTiles(roadPosition);
                    ItemPosition selectedTile =
                        Instantiate(_tileConfigurations[surroundingTiles], roadPosition.transform.position,
                            container.transform.rotation,
                            container);

                    roadPosition.SetRoad(selectedTile);
                }
            }
        }


        /*foreach (ItemPosition itemPosition in itemPositions)
        {
            if (itemPosition.IsWater || itemPosition.IsElevation)
            {
                continue;
            }

            if (itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
            {
                ItemPosition tile =
                    Instantiate(_roadTileGrey, itemPosition.transform.position,
                        container.transform.rotation,
                        container);

                itemPosition.SetRoad(tile);
                /*itemPosition.EnableRoad();#1#

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                        roadPosition.gameObject.activeInHierarchy)
                    {
                        roadPosition.EnableRoad();
                    }
                }

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy && !roadPosition.IsWater &&
                        roadPosition.gameObject.activeInHierarchy)
                    {
                        string surroundingTiles = CheckSurroundingRoadTiles(roadPosition);
                        ItemPosition selectedTile =
                            Instantiate(_tileConfigurations[surroundingTiles], roadPosition.transform.position,
                                container.transform.rotation,
                                container);

                        roadPosition.SetRoad(selectedTile);
                    }
                }
            }
        }
        */


        foreach (ItemPosition itemPosition in itemPositions)
        {
            if (itemPosition.IsWater || itemPosition.IsElevation)
            {
                continue;
            }

            /*
            if (itemPosition.IsBusy && itemPosition.Item.IsBigHouse)
            {
                ItemPosition tile =
                    Instantiate(_roadTileGrey, itemPosition.transform.position,
                        container.transform.rotation,
                        container);

                itemPosition.SetRoad(tile);
                /*itemPosition.EnableRoad();#1#

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy&&!roadPosition.IsWater&&roadPosition.gameObject.activeInHierarchy)
                    {
                        roadPosition.EnableRoad();
                    }
                }

                foreach (var roadPosition in itemPosition.RoadPositions)
                {
                    if (roadPosition != null && !roadPosition.IsBusy&&!roadPosition.IsWater&&roadPosition.gameObject.activeInHierarchy)
                    {
                        string surroundingTiles = CheckSurroundingRoadTiles(roadPosition);
                        ItemPosition selectedTile =
                            Instantiate(_tileConfigurations[surroundingTiles], roadPosition.transform.position,
                                container.transform.rotation,
                                container);

                        roadPosition.SetRoad(selectedTile);
                    }
                }
            }
            
            yield return  new WaitForSeconds(1f);
            */

            if (!itemPosition.IsBusy && !itemPosition.IsRoad)
            {
                string surroundingTiles = CheckSurroundingTiles(itemPosition);
                ItemPosition selectedTile = Instantiate(_tileConfigurations[surroundingTiles],
                    itemPosition.transform.position, container.transform.rotation, container);

                // Debug.Log("CONTAINER " + container.name);

                itemPosition.SetRoad(selectedTile);
            }

            else
            {
                if (itemPosition.Item != null && itemPosition.Item.IsBigHouse)
                    continue;

                if (itemPosition.IsRoad)
                    continue;

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

    private void ClearRoad(Item item)
    {
        foreach (var road in item.ItemPosition.RoadPositions)
        {
            road.DisableRoad();
        }
    }

    private void ClearRoad(ItemPosition firstItemPositionItem, ItemPosition secondItemPositionItem)
    {
        foreach (var itemPosition in firstItemPositionItem.RoadPositions)
        {
            itemPosition.DisableRoad();
        }

        foreach (var itemPosition in secondItemPositionItem.RoadPositions)
        {
            itemPosition.DisableRoad();
        }
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
        
 Debug.Log("Trail  " +positionsTrail.Count );
 Debug.Log("Road  " +positionsRoad.Count );
 
        foreach (var position in positionsTrail)
        {
            Debug.Log("строим  1");
      
            
            string trail = CheckSurroundingTilesSandBox(position);
            Debug.Log("строим  3" + trail);
            Debug.Log("pos" + position.transform.position);
            Debug.Log("contziner  3" + container);
            Debug.Log("tile  3" + _tileConfigurations[trail]);
           
            ItemPosition selectedTile =
                Instantiate(_tileConfigurations[trail], position.transform.position, container.rotation, container);
Debug.Log("строим  5");
            position.SetRoad(selectedTile);
            Debug.Log("строим  6");
        }
        
        Debug.Log("строим  ");
        
        foreach (var position in positionsRoad)
        {
            string trail = CheckSurroundingTilesRoadsSandBox(position);
            ItemPosition selectedTile =
                Instantiate(_tileConfigurations[trail], position.transform.position, container.rotation, container);

            position.SetRoad(selectedTile);
        }
        
        Debug.Log("делаем   ");
    }

    private string CheckSurroundingTilesSandBox(ItemPosition itemPosition)
    {
        float value = 0;

        string surroundingTiles = "0000";

        if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy &&
            itemPosition.NorthPosition.gameObject.activeInHierarchy && !itemPosition.NorthPosition.IsWater &&
            !itemPosition.NorthPosition.IsRoad && itemPosition.NorthPosition.IsTrail)
        {
            value++;
            Debug.Log("Нашлась тропа тут " + itemPosition.name);
            surroundingTiles = "1" + surroundingTiles.Substring(1);
        }

        if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy &&
            itemPosition.WestPosition.gameObject.activeInHierarchy && !itemPosition.WestPosition.IsWater &&
            !itemPosition.WestPosition.IsRoad && itemPosition.WestPosition.IsTrail)
        {
            value++;
            Debug.Log("Нашлась тропа тут " + itemPosition.name);
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("West " + itemPosition.WestPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 1) + "1" + surroundingTiles.Substring(2);
        }

        if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy &&
            itemPosition.EastPosition.gameObject.activeInHierarchy && !itemPosition.EastPosition.IsWater &&
            !itemPosition.EastPosition.IsRoad && itemPosition.EastPosition.IsTrail)
        {
            value++;
            Debug.Log("Нашлась тропа тут " + itemPosition.name);
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("East " + itemPosition.EastPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 2) + "1" + surroundingTiles.Substring(3);
        }

        if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy &&
            itemPosition.SouthPosition.gameObject.activeInHierarchy && !itemPosition.SouthPosition.IsWater &&
            !itemPosition.SouthPosition.IsRoad && itemPosition.SouthPosition.IsTrail)
        {
            value++;
            Debug.Log("Нашлась тропа тут " + itemPosition.name);
            surroundingTiles = surroundingTiles.Substring(0, surroundingTiles.Length - 1) + "1";
        }

        if (value < 1)
        {
            Debug.Log("NOTraiel " + itemPosition.name);
            return "0000";
        }

        Debug.Log("Traiel " + itemPosition.name);
        return surroundingTiles;
    }

    public void GenerateSandBoxRoad(ItemPosition[] itemPositions, Transform container)
    {
        List<ItemPosition> positionsRoad = new List<ItemPosition>();

        foreach (var itemPosition in itemPositions)
        {
            if (itemPosition.IsRoad)
            {
                Debug.Log("сколько позиций " + positionsRoad.Count);
                positionsRoad.Add(itemPosition);
            }
        }

        foreach (var position in positionsRoad)
        {
            string trail = CheckSurroundingTilesRoadsSandBox(position);
            ItemPosition selectedTile =
                Instantiate(_tileConfigurations[trail], position.transform.position, container.rotation, container);

            position.SetRoad(selectedTile);
        }
    }

    private string CheckSurroundingTilesRoadsSandBox(ItemPosition itemPosition)
    {
        string surroundingTiles = "3333";
        int amount = 0;

        if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy &&
            itemPosition.NorthPosition.gameObject.activeInHierarchy && !itemPosition.NorthPosition.IsWater &&
            itemPosition.NorthPosition.IsRoad)
        {
            amount++;
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("NORTH " + itemPosition.NorthPosition.name );*/
            surroundingTiles = "2" + surroundingTiles.Substring(1);
        }

        if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy &&
            itemPosition.WestPosition.gameObject.activeInHierarchy && !itemPosition.WestPosition.IsWater &&
            itemPosition.WestPosition.IsRoad)
        {
            amount++;
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("West " + itemPosition.WestPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 1) + "2" + surroundingTiles.Substring(2);
        }

        if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy &&
            itemPosition.EastPosition.gameObject.activeInHierarchy && !itemPosition.EastPosition.IsWater &&
            itemPosition.EastPosition.IsRoad)
        {
            amount++;
            /*Debug.Log("Item " + itemPosition.name );
            Debug.Log("East " + itemPosition.EastPosition.name );*/
            surroundingTiles = surroundingTiles.Substring(0, 2) + "2" + surroundingTiles.Substring(3);
        }

        if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy &&
            itemPosition.SouthPosition.gameObject.activeInHierarchy && !itemPosition.SouthPosition.IsWater &&
            itemPosition.SouthPosition.IsRoad)
        {
            amount++;
            surroundingTiles = surroundingTiles.Substring(0, surroundingTiles.Length - 1) + "2";
        }

        if (amount < 1)
        {
            return "0000";
        }

        return surroundingTiles;
    }
}