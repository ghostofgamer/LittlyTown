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

        private int _currentIndex;
        private List<Items> _collectedItems = new List<Items>();
        private int _currentValue;
        
        public List<Items> CollectedItems => _collectedItems;

        private void OnEnable()
        {
            _collectionMovement.PositionScrolled += ActivationDescription;
            _itemThrower.BuildItem += AddItemCollection;
            _merger.ItemMergered += AddItemCollection;
        }

        private void OnDisable()
        {
            _collectionMovement.PositionScrolled -= ActivationDescription;
            _itemThrower.BuildItem -= AddItemCollection;
            _merger.ItemMergered -= AddItemCollection;
        }

        private void Start()
        {
            if (!_collectedItems.Contains(Items.Bush))
                _collectedItems.Add(Items.Bush);

            if (!_collectedItems.Contains(Items.Tree))
                _collectedItems.Add(Items.Tree);

            if (!_collectedItems.Contains(Items.Sawmill))
                _collectedItems.Add(Items.Sawmill);

            LoadCollectedItemsFromPlayerPrefs();
            ShowItems();
            Show();
            ActivationDescription(0);
        }

        public override void Open()
        {
            base.Open();
            _content.SetActive(true);
            Show();
            _camera.orthographic = true;
            ActivationDescription(_currentIndex);
            _environmentMovement.GoAway();
            _cameraMovement.ZoomIn();

            foreach (var item in _itemsContent)
                item.GetComponent<Animator>().SetTrigger(Opened);
        }

        public override void Close()
        {
            base.Close();
            _currentValue = _load.Get(_settingsModes.ToString(), 0);

            if (_currentValue == 0)
            {
                // _camera.orthographicSize = 6;
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
                _collectedItems.Count.ToString() + "/" + _allCollectionItems.Count.ToString();
        }

        private void ShowItems()
        {
            for (int i = 0; i < _allCollectionItems.Count; i++)
            {
                var item = _allCollectionItems[i];

                if (_collectedItems.Contains(item.ItemName))
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
                _lockDescription.gameObject.SetActive(true);
            }
        }

        private void ActivationDescription(int index)
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

        private void AddItemCollection(Item item)
        {
            if (!_collectedItems.Contains(item.ItemName))
            {
                _collectedItems.Add((item.ItemName));
                ShowItems();
                Show();
                SaveCollectedItemsToPlayerPrefs();
            }
        }

        private string SerializeCollectedItemsToJson()
        {
            return JsonConvert.SerializeObject(_collectedItems);
        }

        private void DeserializeCollectedItemsFromJson(string json)
        {
            _collectedItems = JsonConvert.DeserializeObject<List<Items>>(json);
        }

        private void SaveCollectedItemsToPlayerPrefs()
        {
            string json = SerializeCollectedItemsToJson();
            PlayerPrefs.SetString("CollectedItems", json);
        }

        private void LoadCollectedItemsFromPlayerPrefs()
        {
            string json = PlayerPrefs.GetString("CollectedItems");

            if (!string.IsNullOrEmpty(json))
                DeserializeCollectedItemsFromJson(json);
        }
    }
}