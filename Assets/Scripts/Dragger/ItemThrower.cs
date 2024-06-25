using System;
using Enums;
using ItemContent;
using ItemPositionContent;
using Keeper;
using UnityEngine;

namespace Dragger
{
    public class ItemThrower : MonoBehaviour
    {
        [SerializeField] private ItemDragger _itemDragger;
        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Camera _camera;

        private RaycastHit _hit;
        private Ray _ray;

        public event Action PlaceChanged;

        public event Action<ItemPosition> StepCompleted;

        public event Action<Item> BuildItem;

        public ItemPosition LastTrowPosition { get; private set; }

        public void ThrowItem()
        {
            if (_itemKeeper.SelectedObject == null)
                return;

            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _itemDragger.LayerMask))
            {
                if (_hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (!itemPosition.IsBusy && !itemPosition.IsWater && !_itemKeeper.SelectedObject.IsLightHouse)
                    {
                        Throw(itemPosition, _hit);
                    }
                    else if (!itemPosition.IsBusy && itemPosition.IsWater &&
                             _itemKeeper.SelectedObject.IsLightHouse)
                    {
                        Throw(itemPosition, _hit);
                    }
                    else if (itemPosition.IsBusy && _itemDragger.IsObjectSelected)
                    {
                        ReturnPosition();
                        itemPosition.Item.GetComponent<ItemAnimation>().BusyPositionAnimation();
                    }
                    else
                    {
                        ReturnPosition();
                    }
                }
                else
                {
                    ReturnPosition();
                }
            }
        }

        public void ReturnPosition()
        {
            _itemKeeper.SelectedObject.transform.position = _itemKeeper.StartPosition.transform.position;
            _itemKeeper.SelectedObject.Init(_itemKeeper.StartPosition);
            _itemDragger.DisableSelected();
            _itemKeeper.StartPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        }

        private void Throw(ItemPosition itemPosition, RaycastHit hit)
        {
            StepCompleted?.Invoke(itemPosition);
            _itemKeeper.SelectedObject.transform.position = hit.transform.position;
            _itemKeeper.SelectedObject.Init(itemPosition);
            _itemKeeper.SelectedObject.Activation();
            _audioSource.PlayOneShot(_audioSource.clip);
            _itemKeeper.SelectedObject.GetComponent<ItemAnimation>().PositioningAnimation();
            BuildItem?.Invoke(_itemKeeper.SelectedObject);
            itemPosition.DeliverObject(_itemKeeper.SelectedObject);
            PlaceChanged?.Invoke();

            if (_itemKeeper.SelectedObject.ItemName == Items.LightHouse ||
                _itemKeeper.SelectedObject.ItemName == Items.Crane)
                _itemKeeper.InstallItemForLastPosition();

            _itemKeeper.ClearSelectedItem();
            LastTrowPosition = itemPosition;
            _itemDragger.DisableSelected();
        }
    }
}