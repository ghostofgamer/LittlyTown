using System.Collections;
using System.Collections.Generic;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

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

    private void OnEnable()
    {
        _environmentBuilder.EnvironmentBuilded += SaveChanges;
        _itemBuilder.ItemBuilded += SaveChanges;
        _cleaner.ItemRemoved += SaveChanges;
    }

    private void OnDisable()
    {
        _environmentBuilder.EnvironmentBuilded -= SaveChanges;
        _itemBuilder.ItemBuilded -= SaveChanges;
        _cleaner.ItemRemoved -= SaveChanges;
    }

    public void SaveChanges()
    {
        Debug.Log("Сохраняет ");

        if (_coroutineSave != null)
            StopCoroutine(_coroutineSave);

        _coroutineSave = StartCoroutine(SaveDataInfo());
    }

    private IEnumerator SaveDataInfo()
    {
        yield return new WaitForSeconds(0.5f);
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
            ItemPositionData itemPositionData = new ItemPositionData(itemPosition.IsWater, itemPosition.IsBusy,
                itemPosition.IsElevation, itemPosition.IsTrail, itemPosition.IsRoad);
            itemPositions.Add(itemPositionData);
        }

        sandBoxSaveData.ItemDatas = itemDatas;
        sandBoxSaveData.ItemPositionDatas = itemPositions;
        string jsonData = JsonUtility.ToJson(sandBoxSaveData);
        PlayerPrefs.SetString(SandBoxSave, jsonData);
        PlayerPrefs.Save();
        Debug.Log("Sandboxсохранение");
        yield return null;
    }

    public void LoadDataInfo()
    {
        if (_coroutineLoad != null)
            StopCoroutine(_coroutineLoad);

        _coroutineLoad = StartCoroutine(LoadData());
    }

    private IEnumerator LoadData()
    {
        SandBoxSaveData sandBoxSaveData = new SandBoxSaveData();
        List<ItemPosition> _isWaterPosition = new List<ItemPosition>();
        List<ItemPosition> _isElevationPosition = new List<ItemPosition>();

        if (PlayerPrefs.HasKey(SandBoxSave))
        {
            string jsonData = PlayerPrefs.GetString(SandBoxSave);
            sandBoxSaveData = JsonUtility.FromJson<SandBoxSaveData>(jsonData);

            for (int i = 0; i < _itemPositions.Length; i++)
            {
                _itemPositions[i].Init(sandBoxSaveData.ItemPositionDatas[i].IsBusy,
                    sandBoxSaveData.ItemPositionDatas[i].IsElevation, sandBoxSaveData.ItemPositionDatas[i].IsWater,
                    sandBoxSaveData.ItemPositionDatas[i].IsRoad, sandBoxSaveData.ItemPositionDatas[i].IsTrail);

                if (_itemPositions[i].IsWater)
                    _isWaterPosition.Add(_itemPositions[i]);
                
                if (_itemPositions[i].IsElevation)
                    _isElevationPosition.Add(_itemPositions[i]);
            }

            foreach (var waterPosition in _isWaterPosition)
            {
                _environmentBuilder.CreateWater(waterPosition);
            }
            
            yield return new WaitForSeconds(0.5f);
            
            foreach (var elevationPosition in _isElevationPosition)
            {
                // _environmentBuilder.CreateEnvironment(_environmentBuilder.IsTileElevation,elevationPosition);
                
                ItemPosition itemPositionTile;
                Vector3 newLocalPosition = new Vector3(elevationPosition.transform.localPosition.x, 2.1f,
                    elevationPosition.transform.localPosition.z);
                elevationPosition.transform.localPosition = newLocalPosition;

                itemPositionTile = Instantiate(_environmentBuilder.IsTileElevation, elevationPosition.transform.position,
                    Quaternion.identity, _roadContainer);

                itemPositionTile.transform.localPosition = new Vector3(
                    itemPositionTile.transform.localPosition.x, 4.3f,
                    itemPositionTile.transform.localPosition.z);
                
                
                elevationPosition.SetRoad(itemPositionTile);
            }

            foreach (var itemData in sandBoxSaveData.ItemDatas)
            {
                if (itemData != null)
                {
                    if (itemData.ItemName != Items.Crane)
                    {
                        Item item = Instantiate(GetItem(itemData.ItemName), itemData.ItemPosition.transform.position,
                            _itemContainer.transform.rotation, _itemContainer);
                        item.Init(itemData.ItemPosition);
                        item.Activation();
                    }
                }
            }

            if (_roadGenerator != null)
            {
                Debug.Log("utythfnjh tcnm");
            }
            else
            {
                Debug.Log("нету");
            }

            yield return new WaitForSeconds(1f);

            _roadGenerator.GenerateSandBoxTrail(_itemPositions, _roadContainer);
            // gameObject.SetActive(false);
        }
        else
        {
            // gameObject.SetActive(false);
            Debug.Log("нет сохранения");
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