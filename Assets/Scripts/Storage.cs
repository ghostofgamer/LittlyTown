using Dragger;
using ItemContent;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Spawner _spawner;

    private Item _currentItem;
    private Item _temporaryItem;

    public void ChangeItem()
    {
        if (_currentItem == null)
        {
            if (_itemDragger.TemporaryItem == null)
            {
                Debug.Log("Если null ");
                _currentItem = _itemDragger.SelectedObject;
                _currentItem.gameObject.SetActive(false);
                _itemDragger.ClearItem();
                _spawner.OnCreateItem();
                _image.gameObject.SetActive(true);
                _image.sprite = _currentItem.ItemDropDataSo.Icon;
            }
            else
            {
                Debug.Log("Если неее null ");
                _currentItem = _itemDragger.SelectedObject;
                _currentItem.gameObject.SetActive(false);
                _itemDragger.ClearItem();
                _itemDragger.SetItem(_itemDragger.TemporaryItem,_currentItem.ItemPosition);
                _itemDragger.TemporaryItem.gameObject.SetActive(true);
                _itemDragger.ClearTemporaryItem();
                _image.gameObject.SetActive(true);
                _image.sprite = _currentItem.ItemDropDataSo.Icon;
            }

            Debug.Log("Если null " + _currentItem.name);
            // _itemDragger.SelectedObject.gameObject.SetActive(false);
        }
        else
        {
            _temporaryItem = _itemDragger.SelectedObject;
            Debug.Log("Есть " + _temporaryItem.name);
            _temporaryItem.gameObject.SetActive(false);
            _itemDragger.SetItem(_currentItem, _temporaryItem.ItemPosition);
            _currentItem.gameObject.SetActive(true);
            _image.sprite = _temporaryItem.ItemDropDataSo.Icon;
            _currentItem = _temporaryItem;
        }
    }
}