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

                    _currentLookPosition = itemPosition;

                    if (_itemDragger.IsObjectSelected)
                        _currentLookPosition.GetComponent<VisualItemPosition>().ActivateVisual();

                    /*if (_currentLookPosition.IsBusy)
                        return;*/
                    Debug.Log("имя " + _currentLookPosition.name);
                    PlaceLooking?.Invoke(_currentLookPosition, _itemDragger.SelectedObject);
                }
            }
        }
    }
}