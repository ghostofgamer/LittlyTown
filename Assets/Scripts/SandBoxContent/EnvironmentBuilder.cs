using System;
using Enums;
using ItemPositionContent;
using UnityEngine;

public class EnvironmentBuilder : Builder
{
    [SerializeField] private ItemPosition _tileElevation;
    [SerializeField] private GameObject _environment;

    private bool _isTileClear;
    private bool _isTileWater;
    private bool _isTileElevation;
    private bool _isTileTrail;
    private bool _isTileRoad;
    
    public event Action EnvironmentBuilded;
    
    public ItemPosition IsTileElevation => _tileElevation;
    
    /*protected override void Start()
    {
        base.Start();

        /*
        foreach (var itemPosition in ItemPositions)
            itemPosition.GetComponent<FinderPositions>().FindNeighbor();

        _environment.SetActive(false);#1#
    }*/

    private void DeactivateEnvironment()
    {
        _isTileClear = false;
        _isTileWater = false;
        _isTileElevation = false;
        _isTileTrail = false;
        _isTileRoad = false;
    }

    public void ChangeEnvironment(Environments environmentName)
    {
        string name = environmentName.ToString();
        DeactivateEnvironment();

        switch (name)
        {
            case "ClearTile":
                _isTileClear = true;
                break;
            case "Water":
                _isTileWater = true;
                break;
            case "Elevation":
                _isTileElevation = true;
                break;
            case "Trail":
                _isTileTrail = true;
                break;
            case "Road":
                _isTileRoad = true;
                break;
        }
    }

    protected override void FirstChoose()
    {
        ChangeEnvironment(Environments.ClearTile);
    }

    protected override void TakeAction(ItemPosition itemPosition)
    {
        ClearPosition(itemPosition);
        ChangeElevationPosition(itemPosition);
        itemPosition.DeactivationAll();

        if (_isTileClear)
        {
            CreateEnvironment(ClearTile, itemPosition);
        }

        if (_isTileWater)
        {
            CreateWater(itemPosition);
        }

        if (_isTileElevation)
        {
            CreateEnvironment(_tileElevation, itemPosition);
            itemPosition.OnElevation();
        }

        if (_isTileTrail)
        {
            if (itemPosition.IsTrail)
                return;

            itemPosition.ActivateTrail();
        }

        if (_isTileRoad)
        {
            if (itemPosition.IsRoad)
                return;

            itemPosition.EnableRoad();
        }

        StartRoadGeneration();
        EnvironmentBuilded?.Invoke();
    }

    private void ClearPosition(ItemPosition itemPosition)
    {
        if (itemPosition.IsBusy)
        {
            itemPosition.Item.gameObject.SetActive(false);
            itemPosition.ClearingPosition();
        }
    }

    public void CreateWater(ItemPosition itemPosition)
    {
        CreateEnvironment(TileWater, itemPosition);
        itemPosition.ActivationWater();
    }

    private void ChangeElevationPosition(ItemPosition itemPosition)
    {
        if (_isTileElevation)
            return;

        if (itemPosition.IsElevation)
        {
            Vector3 newLocalPosition = new Vector3(itemPosition.transform.localPosition.x, 0.59f,
                itemPosition.transform.localPosition.z);
            itemPosition.transform.localPosition = newLocalPosition;
        }
    }

    public void CreateEnvironment(ItemPosition tilePrefab, ItemPosition itemPosition)
    {
        ItemPosition itemPositionTile;

        if (_isTileElevation)
        {
            Vector3 newLocalPosition = new Vector3(itemPosition.transform.localPosition.x, 2.1f,
                itemPosition.transform.localPosition.z);
            itemPosition.transform.localPosition = newLocalPosition;

            itemPositionTile = Instantiate(tilePrefab, itemPosition.transform.position,
                Quaternion.identity, RoadContainer);

            itemPositionTile.transform.localPosition = new Vector3(
                itemPositionTile.transform.localPosition.x, 4.3f,
                itemPositionTile.transform.localPosition.z);
        }
        else
        {
            itemPositionTile = Instantiate(tilePrefab, itemPosition.transform.position,
                Quaternion.identity, RoadContainer);
        }

        itemPosition.SetRoad(itemPositionTile);
    }
}