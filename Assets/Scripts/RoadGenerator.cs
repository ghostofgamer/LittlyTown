using System.Collections;
using System.Collections.Generic;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private ItemPosition[] _itemPositions;
    [SerializeField] private Merger _merger;

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
        _spawner.ItemCreated += OnGeneration;
        // _merger.Merged += OnGeneration;
    }

    private void OnDisable()
    {
        _spawner.ItemCreated -= OnGeneration;
        // _merger.Merged -= OnGeneration;
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
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CreateRoad());
    }

    private IEnumerator CreateRoad()
    {
        yield return _waitForSeconds;

        foreach (ItemPosition itemPosition in _itemPositions)
        {
            if (itemPosition.IsWater||itemPosition.IsElevation)
            {
                continue;
            }
              
            
            if (!itemPosition.IsBusy)
            {
                string surroundingTiles = CheckSurroundingTiles(itemPosition);
                ItemPosition selectedTile = Instantiate(_tileConfigurations[surroundingTiles],
                    itemPosition.transform.position, Quaternion.identity);

                itemPosition.SetRoad(selectedTile);
            }

            else
            {
                ItemPosition selectedTile =
                    Instantiate(_clearTile, itemPosition.transform.position, Quaternion.identity);

                itemPosition.SetRoad(selectedTile);
            }

            yield return null;
        }
    }

    private string CheckSurroundingTiles(ItemPosition itemPosition)
    {
        string surroundingTiles = "0000";

        if (itemPosition.NorthPosition != null && !itemPosition.NorthPosition.IsBusy)
        {
            surroundingTiles = "1" + surroundingTiles.Substring(1);
        }

        if (itemPosition.WestPosition != null && !itemPosition.WestPosition.IsBusy)
        {
            surroundingTiles = surroundingTiles.Substring(0, 1) + "1" + surroundingTiles.Substring(2);
        }

        if (itemPosition.EastPosition != null && !itemPosition.EastPosition.IsBusy)
        {
            surroundingTiles = surroundingTiles.Substring(0, 2) + "1" + surroundingTiles.Substring(3);
        }

        if (itemPosition.SouthPosition != null && !itemPosition.SouthPosition.IsBusy)
        {
            surroundingTiles = surroundingTiles.Substring(0, surroundingTiles.Length - 1) + "1";
        }

        return surroundingTiles;
    }
}