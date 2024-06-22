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

        public ItemPosition LastTrowPosition { get; private set; }

        public event Action PlaceChanged;

        public event Action<ItemPosition> StepCompleted;

        public event Action<Item> BuildItem;

        public void ThrowItem()
        {
            if (_itemKeeper.SelectedObject == null)
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _itemDragger.LayerMask))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (!itemPosition.IsBusy && !itemPosition.IsWater && !_itemKeeper.SelectedObject.IsLightHouse)
                    {
                        Throw(itemPosition, hit);
                    }
                    else if (!itemPosition.IsBusy && itemPosition.IsWater && _itemKeeper.SelectedObject.IsLightHouse)
                    {
                        Throw(itemPosition, hit);
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

        private void ReturnPosition()
        {
            _itemKeeper.SelectedObject.transform.position = _itemKeeper.StartPosition.transform.position;
            _itemKeeper.SelectedObject.Init(_itemKeeper.StartPosition);
            _itemDragger.DisableSelected();
            Debug.Log("LOK");
            _itemKeeper.StartPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        }
    }
}