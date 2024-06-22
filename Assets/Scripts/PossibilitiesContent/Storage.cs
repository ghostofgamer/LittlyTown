using System;
using ItemContent;
using Keeper;
using SpawnContent;
using UnityEngine;
using UnityEngine.UI;

namespace PossibilitiesContent
{
    public class Storage : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private Spawner _spawner;

        private Item _currentItem;
        private Item _temporaryItem;

        public event Action StoragePlaceChanged;

        public Item CurrentItem => _currentItem;

        public void ChangeItem()
        {
            if (_currentItem == null)
            {
                if (_itemKeeper.TemporaryItem == null)
                {
                    KeepCurrentItem();
                    _spawner.OnCreateItem();
                    ActivateItemIcon();
                }
                else
                {
                    KeepCurrentItem();
                    _itemKeeper.SetItem(_itemKeeper.TemporaryItem, _currentItem.ItemPosition);
                    _itemKeeper.TemporaryItem.gameObject.SetActive(true);
                    _itemKeeper.ClearTemporaryItem();
                    ActivateItemIcon();
                }
            }
            else
            {
                _temporaryItem = _itemKeeper.SelectedObject;
                _temporaryItem.gameObject.SetActive(false);
                _itemKeeper.SetItem(_currentItem, _temporaryItem.ItemPosition);
                _currentItem.gameObject.SetActive(true);
                _image.sprite = _temporaryItem.ItemDropDataSo.Icon;
                _currentItem = _temporaryItem;
            }

            StoragePlaceChanged?.Invoke();
        }

        private void KeepCurrentItem()
        {
            _currentItem = _itemKeeper.SelectedObject;
            _currentItem.gameObject.SetActive(false);
            _itemKeeper.ClearSelectedItem();
        }

        private void ActivateItemIcon()
        {
            _image.gameObject.SetActive(true);
            _image.sprite = _currentItem.ItemDropDataSo.Icon;
        }

        public void SetItem(Item item)
        {
            if (item != null)
            {
                _image.gameObject.SetActive(true);
                _currentItem = item;
                _temporaryItem = null;
                _image.sprite = _currentItem.ItemDropDataSo.Icon;
            }
            else
            {
                _image.gameObject.SetActive(false);
                _currentItem = null;
                _temporaryItem = null;
                _image.sprite = null;
            }
        }

        public void ClearItem()
        {
            _currentItem = null;
            _temporaryItem = null;
            _image.gameObject.SetActive(false);
        }
    }
}