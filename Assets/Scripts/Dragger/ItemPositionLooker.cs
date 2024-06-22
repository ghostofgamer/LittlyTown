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

        private ItemPosition _currentLookPosition = null;
        private ItemDragger _itemDragger;

        public event Action<ItemPosition, Item> PlaceLooking;

        private void Start()
        {
            _itemDragger = GetComponent<ItemDragger>();
        }

        public void LookPosition()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _itemDragger.LayerMask))
            {
                if (hit.transform.gameObject.TryGetComponent(out ItemPosition itemPosition))
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