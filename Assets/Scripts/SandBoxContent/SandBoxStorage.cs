using System.Collections;
using System.Collections.Generic;
using Enums;
using ItemContent;
using ItemPositionContent;
using Road;
using SaveAndLoad;
using UnityEngine;

namespace SandBoxContent
{
    public class SandBoxStorage : MonoBehaviour
    {
        private const string SandBoxSave = "SandBoxSave";

        [SerializeField] private ItemPosition[] _itemPositions;
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private Transform _roadContainer;
        [SerializeField] private Item[] _items;
        [SerializeField] private Cleaner _cleaner;
        [SerializeField] private ItemBuilder _itemBuilder;
        [SerializeField] private EnvironmentBuilder _environmentBuilder;
        [SerializeField] private RoadGenerator _roadGenerator;

        private Coroutine _coroutineSave;
        private Coroutine _coroutineLoad;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);
        private WaitForSeconds _waitForMoment = new WaitForSeconds(1f);
        private float _maxPositionElevationY = 2.1f;
        private float _positionElevationTileY = 4.3f;

        private void OnEnable()
        {
            _environmentBuilder.EnvironmentBuilded += OnSaveChanges;
            _itemBuilder.ItemBuilded += OnSaveChanges;
            _cleaner.ItemRemoved += OnSaveChanges;
        }

        private void OnDisable()
        {
            _environmentBuilder.EnvironmentBuilded -= OnSaveChanges;
            _itemBuilder.ItemBuilded -= OnSaveChanges;
            _cleaner.ItemRemoved -= OnSaveChanges;
        }

        public void LoadDataInfo()
        {
            if (_coroutineLoad != null)
                StopCoroutine(_coroutineLoad);

            _coroutineLoad = StartCoroutine(LoadData());
        }

        private void OnSaveChanges()
        {
            if (_coroutineSave != null)
                StopCoroutine(_coroutineSave);

            _coroutineSave = StartCoroutine(SaveDataInfo());
        }

        private IEnumerator SaveDataInfo()
        {
            yield return _waitForSeconds;
            SandBoxSaveData sandBoxSaveData = new SandBoxSaveData();
            List<ItemData> itemDatas = new List<ItemData>();
            List<ItemPositionData> itemPositions = new List<ItemPositionData>();

            foreach (var itemPosition in _itemPositions)
            {
                if (itemPosition.Item != null)
                {
                    ItemData itemData = new ItemData(itemPosition.Item.ItemName, itemPosition);
                    itemDatas.Add(itemData);
                }
            }

            foreach (var itemPosition in _itemPositions)
            {
                ItemPositionData itemPositionData = new ItemPositionData(
                    itemPosition.IsWater,
                    itemPosition.IsBusy,
                    itemPosition.IsElevation,
                    itemPosition.IsTrail,
                    itemPosition.IsRoad);
                itemPositions.Add(itemPositionData);
            }

            sandBoxSaveData.ItemDatas = itemDatas;
            sandBoxSaveData.ItemPositionDatas = itemPositions;
            string jsonData = JsonUtility.ToJson(sandBoxSaveData);
            PlayerPrefs.SetString(SandBoxSave, jsonData);
            PlayerPrefs.Save();
            yield return null;
        }

        private IEnumerator LoadData()
        {
            SandBoxSaveData sandBoxSaveData = new SandBoxSaveData();
            List<ItemPosition> isWaterPosition = new List<ItemPosition>();
            List<ItemPosition> isElevationPosition = new List<ItemPosition>();

            if (PlayerPrefs.HasKey(SandBoxSave))
            {
                string jsonData = PlayerPrefs.GetString(SandBoxSave);
                sandBoxSaveData = JsonUtility.FromJson<SandBoxSaveData>(jsonData);

                for (int i = 0; i < _itemPositions.Length; i++)
                {
                    _itemPositions[i].Init(
                        sandBoxSaveData.ItemPositionDatas[i].IsBusy,
                        sandBoxSaveData.ItemPositionDatas[i].IsElevation,
                        sandBoxSaveData.ItemPositionDatas[i].IsWater,
                        sandBoxSaveData.ItemPositionDatas[i].IsRoad,
                        sandBoxSaveData.ItemPositionDatas[i].IsTrail);

                    if (_itemPositions[i].IsWater)
                        isWaterPosition.Add(_itemPositions[i]);

                    if (_itemPositions[i].IsElevation)
                        isElevationPosition.Add(_itemPositions[i]);
                }

                foreach (var waterPosition in isWaterPosition)
                    _environmentBuilder.CreateWater(waterPosition);

                yield return _waitForSeconds;

                foreach (var elevationPosition in isElevationPosition)
                {
                    ItemPosition itemPositionTile;
                    Vector3 newLocalPosition = new Vector3(
                        elevationPosition.transform.localPosition.x,
                        _maxPositionElevationY,
                        elevationPosition.transform.localPosition.z);
                    elevationPosition.transform.localPosition = newLocalPosition;
                    itemPositionTile = Instantiate(
                        _environmentBuilder.IsTileElevation,
                        elevationPosition.transform.position,
                        Quaternion.identity,
                        _roadContainer);
                    itemPositionTile.transform.localPosition = new Vector3(
                        itemPositionTile.transform.localPosition.x,
                        _positionElevationTileY,
                        itemPositionTile.transform.localPosition.z);
                    elevationPosition.SetRoad(itemPositionTile);
                }

                foreach (var itemData in sandBoxSaveData.ItemDatas)
                {
                    if (itemData != null)
                    {
                        if (itemData.ItemName != Items.Crane)
                        {
                            Item item = Instantiate(
                                GetItem(itemData.ItemName),
                                itemData.ItemPosition.transform.position,
                                _itemContainer.transform.rotation,
                                _itemContainer);
                            item.Init(itemData.ItemPosition);
                            item.Activation();
                        }
                    }
                }

                yield return _waitForMoment;
                _roadGenerator.GenerateSandBoxTrail(_itemPositions, _roadContainer);
            }
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
    }
}