using UnityEngine;

namespace Dragger
{
    [RequireComponent(typeof(ItemDragger))]
    public class InputItemDragger : MonoBehaviour
    {
        private ItemDragger _itemDragger;

        private void Awake()
        {
            enabled = false;
            _itemDragger = GetComponent<ItemDragger>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _itemDragger.SelectItem();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _itemDragger.ThrowItem();
            }

            if (_itemDragger.IsObjectSelected || _itemDragger.IsPositionSelected)
            {
                _itemDragger.DragItem();
            }
        }
    }
}