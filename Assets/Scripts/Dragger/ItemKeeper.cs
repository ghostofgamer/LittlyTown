using System.Collections;
using System.Collections.Generic;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

public class ItemKeeper : MonoBehaviour
{
    public Item SelectedObject { get; private set; }

    public Item TemporaryItem { get; private set; }
    
    public ItemPosition StartPosition { get; private set; }
    
    /*
    public void SetTemporaryItem(Item item)
    {
        TemporaryItem = SelectedObject;
        TemporaryItem.gameObject.SetActive(false);
        SetItem(item, StartPosition);
        SelectedObject.gameObject.SetActive(true);
    }
    */

    public void ClearTemporaryItem()
    {
        TemporaryItem = null;
    }

    public void SetTemporaryObject(Item item)
    {
        TemporaryItem = item;

        if (TemporaryItem != null)
            TemporaryItem.gameObject.SetActive(false);
    }
    
    /*public void SetItem(Item item, ItemPosition itemPosition)
    {
        SelectedObject = item;
        StartPosition = itemPosition;
        SelectedObject.Init(StartPosition);
        StartPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        SelectNewItem?.Invoke();
    }*/
}
