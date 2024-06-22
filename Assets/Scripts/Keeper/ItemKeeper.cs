using System;
using System.Collections;
using Dragger;
using ItemContent;
using ItemPositionContent;
using UnityEngine;

public class ItemKeeper : MonoBehaviour
{
    [SerializeField] private ItemThrower _itemThrower;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private LookMerger _lookMerger;

    public event Action SelectNewItem;

    public Item SelectedObject { get; private set; }

    public Item TemporaryItem { get; private set; }

    public ItemPosition StartPosition { get; private set; }

    private void OnEnable()
    {
        _lookMerger.NotMerged += InstallItemForLastPosition;
    }

    private void OnDisable()
    {
        _lookMerger.NotMerged -= InstallItemForLastPosition;
    }

    public void SetTemporaryItem(Item item)
    {
        TemporaryItem = SelectedObject;
        TemporaryItem.gameObject.SetActive(false);
        SetItem(item, StartPosition);
        SelectedObject.gameObject.SetActive(true);
    }

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

    public void SetItem(Item item, ItemPosition itemPosition)
    {
        SelectedObject = item;
        StartPosition = itemPosition;
        SelectedObject.Init(StartPosition);
        StartPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        SelectNewItem?.Invoke();
    }

    public void ChangeStartPosition(ItemPosition itemPosition)
    {
        itemPosition.GetComponent<VisualItemPosition>().ActivateVisual();
        StartPosition = itemPosition;
        SelectedObject.transform.position = itemPosition.transform.position;
    }


    public void ClearAll()
    {
        if (SelectedObject != null)
        {
            SelectedObject.gameObject.SetActive(false);
            SelectedObject = null;
        }

        if (TemporaryItem != null)
        {
            TemporaryItem.gameObject.SetActive(false);
            TemporaryItem = null;
        }

        if (StartPosition != null)
        {
            StartPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
            StartPosition = null;
        }
    }

    public void SwitchOff()
    {
        if (SelectedObject != null)
            SelectedObject.gameObject.SetActive(false);

        if (StartPosition != null)
            StartPosition.GetComponent<VisualItemPosition>().DeactivateVisual();
    }

    public void SwitchOn()
    {
        if (SelectedObject != null)
            SelectedObject.gameObject.SetActive(true);

        if (StartPosition != null)
            StartPosition.GetComponent<VisualItemPosition>().ActivateVisual();
    }

    public void ClearItem()
    {
        SelectedObject = null;
    }

    public void InstallItemForLastPosition()
    {
        StartCoroutine(StartSetItem(_itemThrower.LastTrowPosition));
    }

    private IEnumerator StartSetItem(ItemPosition itemPosition)
    {
        yield return new WaitForSeconds(0.1f);

        if (TemporaryItem != null)
        {
            if (itemPosition == StartPosition)
            {
                ItemPosition newPosition = _spawner.GetPosition();

                if (newPosition != null)
                {
                    SetItem(TemporaryItem, newPosition);
                    TemporaryItem = null;
                    SelectedObject.gameObject.SetActive(true);
                }
            }
            else
            {
                SetItem(TemporaryItem, StartPosition);
                TemporaryItem = null;
                SelectedObject.gameObject.SetActive(true);
            }
        }
    }
}