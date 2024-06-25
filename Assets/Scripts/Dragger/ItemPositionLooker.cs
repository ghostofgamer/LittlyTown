using System;
using ItemContent;
using ItemPositionContent;
using Keeper;
using UnityEngine;

namespace Dragger
{
    [RequireComponent(typeof(ItemDragger))]
    public class ItemPositionLooker : MonoBehaviour
    {
        [SerializeField] private ItemKeeper _itemKeeper;
        [SerializeField] private Camera _camera;

        private ItemPosition _currentLookPosition = null;
        private ItemDragger _itemDragger;
        private RaycastHit _hit;
        private Ray _ray;

        public event Action<ItemPosition, Item> PlaceLooking;

        private void Start()
        {
            _itemDragger = GetComponent<ItemDragger>();
        }

        public void LookPosition()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _itemDragger.LayerMask))
            {
                if (_hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
                {
                    if (_currentLookPosition == itemPosition)
                        return;

                    _currentLookPosition = itemPosition;

                    if (_itemDragger.IsObjectSelected)
                        _currentLookPosition.GetComponent<VisualItemPosition>().ActivateVisual();

                    PlaceLooking?.Invoke(_currentLookPosition, _itemKeeper.SelectedObject);
                }
            }
        }
    }
}