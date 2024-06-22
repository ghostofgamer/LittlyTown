using UnityEngine;

namespace ItemContent
{
    [RequireComponent(typeof(Item))]
    public class ItemStay : MonoBehaviour
    {
        private Item _item;

        private void Start()
        {
            _item = GetComponent<Item>();
        }

        private void Update()
        {
            if (_item.ItemPosition == null)
                return;

            if (transform.position != _item.ItemPosition.transform.position)
            {
                transform.position = _item.ItemPosition.transform.position;
                transform.forward = _item.ItemPosition.transform.forward;
            }
        }
    }
}