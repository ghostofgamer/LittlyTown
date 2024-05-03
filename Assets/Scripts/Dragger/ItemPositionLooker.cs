using System;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

namespace Dragger
{
    public class ItemPositionLooker : MonoBehaviour
    {
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

                    if (_currentLookPosition != null)
                    {
                        _currentLookPosition.ClearingPosition();
                    }

                    _currentLookPosition = itemPosition;
                    _currentLookPosition.GetComponent<VisualItemPosition>().ActivateVisual();
                    _currentLookPosition.SetSelected();
                    PlaceLooking?.Invoke(_currentLookPosition, _itemDragger.SelectedObject);
                }
            }
        }
    }
}
