using System.Collections.Generic;
using CameraContent;
using CollectionContent;
using Dragger;
using Enums;
using EnvironmentContent;
using ItemContent;
using MergeContent;
using Newtonsoft.Json;
using SaveAndLoad;
using TMPro;
using UnityEngine;

namespace UI.Screens
{
    public class CollectionScreen : AbstractScreen
    {
        private const string Opened = "Open";
        private const string CollectedItem = "CollectedItems";

        [SerializeField] private TMP_Text _amountCollectionsItems;
        [SerializeField] private TMP_Text _lockDescription;
        [SerializeField] private GameObject[] _descriptions;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CameraMovement _cameraMovement;
        [SerializeField] private Merger _merger;
        [SerializeField] private EnvironmentMovement _environmentMovement;
        [SerializeField] private CollectionMovement _collectionMovement;
        [SerializeField] private List<Item> _allCollectionItems = new List<Item>();
        [SerializeField] private GameObject _content;
        [SerializeField] private List<Transform> _itemsContent = new List<Transform>();
        [SerializeField] private Camera _camera;
        [SerializeField] private Load _load;
        [SerializeField] private SettingsModes _settingsModes;
        [SerializeField] private ItemThrower _itemThrower;
        [SerializeField] private Save _save;
        [SerializeField] private TMP_Text _amountBuildingsItemText;

        private int _currentIndex;
        private int _currentValue;
        private int _amount;

        public List<Items> CollectedItems { get; private set; } = new List<Items>();

        private void OnEnable()
        {
            _collectionMovement.PositionScrolled += OnActivationDescription;
            _collectionMovement.PositionScrolled += OnShowAmountBuildItem;
            _itemThrower.BuildItem += OnAddItemCollection;
            _itemThrower.BuildItem += OnSaveAmountBuildings;
            _merger.ItemMergered += OnAddItemCollection;
        }

        private void OnDisable()
        {
            _collectionMovement.PositionScrolled -= OnActivationDescription;
            _collectionMovement.PositionScrolled -= OnShowAmountBuildItem;
            _itemThrower.BuildItem -= OnAddItemCollection;
            _itemThrower.BuildItem -= OnSaveAmountBuildings;
            _merger.ItemMergered -= OnAddItemCollection;
        }

        private void Start()
        {
            if (!CollectedItems.Contains(Items.Bush))
                CollectedItems.Add(Items.Bush);

            if (!CollectedItems.Contains(Items.Tree))
                CollectedItems.Add(Items.Tree);

            if (!CollectedItems.Contains(Items.Sawmill))
                CollectedItems.Add(Items.Sawmill);

            LoadCollectedItemsFromPlayerPrefs();
            ShowItems();
            Show();
            OnActivationDescription(0);
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _content.SetActive(true);
            Show();
            _camera.orthographic = true;
            OnActivationDescription(_currentIndex);
            _environmentMovement.GoAway();
            _cameraMovement.ZoomIn();

            foreach (Transform item in _itemsContent)
                item.GetComponent<Animator>().SetTrigger(Opened);
        }

        public override void Close()
        {
            base.Close();
            _currentValue = _load.Get(_settingsModes.ToString(), 0);

            if (_currentValue == 0)
            {
                _camera.orthographicSize = _cameraMovement.StandardOrthographicSize;
                _camera.orthographic = false;
            }
            else
            {
                _camera.orthographic = true;
            }

            _content.SetActive(false);
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _cameraMovement.ResetZoom();
            _environmentMovement.ReturnPosition();
        }

        private void Show()
        {
            _amountCollectionsItems.text =
                CollectedItems.Count + "/" + _allCollectionItems.Count;
        }

        private void ShowItems()
        {
            for (int i = 0; i < _allCollectionItems.Count; i++)
            {
                var item = _allCollectionItems[i];
                item.gameObject.SetActive(CollectedItems.Contains(item.ItemName));
                _lockDescription.gameObject.SetActive(true);
            }
        }

        private void OnActivationDescription(int index)
        {
            _currentIndex = index;

            foreach (GameObject description in _descriptions)
                description.SetActive(false);

            _lockDescription.gameObject.SetActive(false);

            if (_allCollectionItems[index].gameObject.activeSelf)
                _descriptions[index].SetActive(true);
            else
                _lockDescription.gameObject.SetActive(true);
        }

        private void OnAddItemCollection(Item item)
        {
            if (!CollectedItems.Contains(item.ItemName))
            {
                CollectedItems.Add(item.ItemName);
                ShowItems();
                Show();
                SaveCollectedItemsToPlayerPrefs();
            }
        }

        private void OnShowAmountBuildItem(int index)
        {
            _amount = _load.Get(_allCollectionItems[index].ItemName.ToString(), 0);
            _amountBuildingsItemText.text = _amount.ToString();
        }

        private void OnSaveAmountBuildings(Item item)
        {
            _amount = _load.Get(item.ItemName.ToString(), 0);
            _amount++;
            _save.SetData(item.ItemName.ToString(), _amount);
        }

        private string SerializeCollectedItemsToJson()
        {
            return JsonConvert.SerializeObject(CollectedItems);
        }

        private void DeserializeCollectedItemsFromJson(string json)
        {
            CollectedItems = JsonConvert.DeserializeObject<List<Items>>(json);
        }

        private void SaveCollectedItemsToPlayerPrefs()
        {
            string json = SerializeCollectedItemsToJson();
            PlayerPrefs.SetString(CollectedItem, json);
        }

        private void LoadCollectedItemsFromPlayerPrefs()
        {
            string json = PlayerPrefs.GetString(CollectedItem);

            if (!string.IsNullOrEmpty(json))
                DeserializeCollectedItemsFromJson(json);
        }
    }
}