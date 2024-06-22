using System;
using Enums;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

namespace Dragger
{
    public class ItemThrower : MonoBehaviour
    {
        [SerializeField] private ItemDragger _itemDragger;
        [SerializeField] private AudioSource _audioSource;

        public ItemPosition LastTrowPosition { get; private set; }

        public event Action PlaceChanged;

        public event Action<ItemPosition> StepCompleted;

        public event Action<Item> BuildItem;

        public void ThrowItem()
        {
            if (_itemDragger.SelectedObject == null)
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _itemDragger.LayerMask))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (!itemPosition.IsBusy && !itemPosition.IsWater && !_itemDragger.SelectedObject.IsLightHouse)
                    {
                        Throw(itemPosition, hit);
                    }
                    else if (!itemPosition.IsBusy && itemPosition.IsWater && _itemDragger.SelectedObject.IsLightHouse)
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
            _itemDragger.SelectedObject.transform.position = hit.transform.position;
            _itemDragger.SelectedObject.Init(itemPosition);
            _itemDragger.SelectedObject.Activation();
            _audioSource.PlayOneShot(_audioSource.clip);
            _itemDragger.SelectedObject.GetComponent<ItemAnimation>().PositioningAnimation();
            BuildItem?.Invoke(_itemDragger.SelectedObject);
            itemPosition.DeliverObject(_itemDragger.SelectedObject);
            PlaceChanged?.Invoke();

            if (_itemDragger.SelectedObject.ItemName == Items.LightHouse ||
                _itemDragger.SelectedObject.ItemName == Items.Crane)
                _itemDragger.GetItemForLastPosition();
            // StartCoroutine(Continue(itemPosition));

            _itemDragger.ClearItem();
            LastTrowPosition = itemPosition;
            _itemDragger.DisableSelected();
        }

        private void ReturnPosition()
        {
            _itemDragger.SelectedObject.transform.position = _itemDragger.StartPosition.transform.position;
            _itemDragger.SelectedObject.Init(_itemDragger.StartPosition);
            _itemDragger.DisableSelected();
            _itemDragger.StartPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        }
    }
}