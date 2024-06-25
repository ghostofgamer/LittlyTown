using System;
using Enums;
using ItemPositionContent;
using UnityEngine;

namespace SandBoxContent
{
    public class EnvironmentBuilder : Builder
    {
        private const string Clear = "ClearTile";
        private const string Water = "Water";
        private const string Elevation = "Elevation";
        private const string Trail = "Trail";
        private const string Road = "Road";

        [SerializeField] private ItemPosition _tileElevation;

        private bool _isTileClear;
        private bool _isTileWater;
        private bool _isTileElevation;
        private bool _isTileTrail;
        private bool _isTileRoad;
        private Vector3 _newLocalPosition;
        private string _name;
        private float _positionElevationTileY = 4.3f;
        private float _minPositionElevationY = 0.59f;
        private float _maxPositionElevationY = 2.1f;

        public event Action EnvironmentBuilded;

        public ItemPosition IsTileElevation => _tileElevation;

        public void ChangeEnvironment(Environments environmentName)
        {
            _name = environmentName.ToString();
            DeactivateEnvironment();

            switch (_name)
            {
                case Clear:
                    _isTileClear = true;
                    break;
                case Water:
                    _isTileWater = true;
                    break;
                case Elevation:
                    _isTileElevation = true;
                    break;
                case Trail:
                    _isTileTrail = true;
                    break;
                case Road:
                    _isTileRoad = true;
                    break;
            }
        }

        public void CreateWater(ItemPosition itemPosition)
        {
            CreateEnvironment(TileWater, itemPosition);
            itemPosition.ActivationWater();
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

        private void DeactivateEnvironment()
        {
            _isTileClear = false;
            _isTileWater = false;
            _isTileElevation = false;
            _isTileTrail = false;
            _isTileRoad = false;
        }

        private void ClearPosition(ItemPosition itemPosition)
        {
            if (itemPosition.IsBusy)
            {
                itemPosition.Item.gameObject.SetActive(false);
                itemPosition.ClearingPosition();
            }
        }

        private void ChangeElevationPosition(ItemPosition itemPosition)
        {
            if (_isTileElevation)
                return;

            if (itemPosition.IsElevation)
            {
                _newLocalPosition = new Vector3(
                    itemPosition.transform.localPosition.x,
                    _minPositionElevationY,
                    itemPosition.transform.localPosition.z);
                itemPosition.transform.localPosition = _newLocalPosition;
            }
        }

        private void CreateEnvironment(ItemPosition tilePrefab, ItemPosition itemPosition)
        {
            ItemPosition itemPositionTile;

            if (_isTileElevation)
            {
                _newLocalPosition = new Vector3(
                    itemPosition.transform.localPosition.x,
                    _maxPositionElevationY,
                    itemPosition.transform.localPosition.z);
                itemPosition.transform.localPosition = _newLocalPosition;
                itemPositionTile = Instantiate(
                    tilePrefab,
                    itemPosition.transform.position,
                    Quaternion.identity,
                    RoadContainer);
                itemPositionTile.transform.localPosition = new Vector3(
                    itemPositionTile.transform.localPosition.x,
                    _positionElevationTileY,
                    itemPositionTile.transform.localPosition.z);
            }
            else
            {
                itemPositionTile = Instantiate(
                    tilePrefab,
                    itemPosition.transform.position,
                    Quaternion.identity,
                    RoadContainer);
            }

            itemPosition.SetRoad(itemPositionTile);
        }
    }
}