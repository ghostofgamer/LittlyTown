using System.Collections.Generic;
using System.Linq;
using Dragger;
using Enums;
using ItemContent;
using TMPro;
using UI.Screens;
using UnityEngine;
using Newtonsoft.Json;

public class CollectionScreen : AbstractScreen
{
    [SerializeField] private GameObject _backGround;
    [SerializeField] private TMP_Text _amountCollectionsItems;
    [SerializeField] private TMP_Text _lockDescription;
    [SerializeField] private GameObject[] _descriptions;
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Canvas _canvas;

    private CollectionMovement _collectionMovement;
    private int _currentIndex;
    private List<Item> _allCollectionItems = new List<Item>();
    private List<Items> _collectedItems = new List<Items>();
    private GameObject _content;

    private void OnEnable()
    {
        _collectionMovement.PositionScrolled += ActivationDescription;
        _itemDragger.BuildItem += AddItemCollection;
    }

    private void OnDisable()
    {
        _collectionMovement.PositionScrolled -= ActivationDescription;
        _itemDragger.BuildItem -= AddItemCollection;
    }

    private void Start()
    {
        if (!_collectedItems.Contains(Items.Bush))
            _collectedItems.Add(Items.Bush);

        if (!_collectedItems.Contains(Items.Tree))
            _collectedItems.Add(Items.Tree);

        LoadCollectedItemsFromPlayerPrefs();
        ShowItems();
        Show();
        ActivationDescription(0);
    }

    public override void Open()
    {
        base.Open();
        _backGround.SetActive(true);
        _content.SetActive(true);
        Show();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        ActivationDescription(_currentIndex);
    }

    public override void Close()
    {
        base.Close();
        _backGround.SetActive(false);
        _content.SetActive(false);
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    private void Show()
    {
        _amountCollectionsItems.text = _collectedItems.Count.ToString() + " / " + _allCollectionItems.Count.ToString();
    }

    private void ShowItems()
    {
        for (int i = 0; i < _allCollectionItems.Count; i++)
        {
            var item = _allCollectionItems[i];

            if (_collectedItems.Contains(item.ItemName))
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
                _lockDescription.gameObject.SetActive(true);
            }
        }
    }

    private void ActivationDescription(int index)
    {
        _currentIndex = index;
        foreach (GameObject description in _descriptions)
        {
            description.SetActive(false);
        }

        _lockDescription.gameObject.SetActive(false);

        if (_allCollectionItems[index].gameObject.activeSelf)
        {
            _descriptions[index].SetActive(true);
        }
        else
        {
            _lockDescription.gameObject.SetActive(true);
        }
    }

    public void Init(CollectionMovement collectionMovement, List<Item> items, GameObject content)
    {
        _collectionMovement = collectionMovement;
        _allCollectionItems = items;
        _content = content;
    }

    public void AddItemCollection(Item item)
    {
        if (!_collectedItems.Contains(item.ItemName))
        {
            _collectedItems.Add((item.ItemName));
            ShowItems();
            Show();
            SaveCollectedItemsToPlayerPrefs();
        }
    }

    public string SerializeCollectedItemsToJson()
    {
        return JsonConvert.SerializeObject(_collectedItems);
    }

    public void DeserializeCollectedItemsFromJson(string json)
    {
        _collectedItems = JsonConvert.DeserializeObject<List<Items>>(json);
    }

    public void SaveCollectedItemsToPlayerPrefs()
    {
        string json = SerializeCollectedItemsToJson();
        PlayerPrefs.SetString("CollectedItems", json);
    }

    public void LoadCollectedItemsFromPlayerPrefs()
    {
        string json = PlayerPrefs.GetString("CollectedItems");
        if (!string.IsNullOrEmpty(json))
        {
            DeserializeCollectedItemsFromJson(json);
        }
    }
}