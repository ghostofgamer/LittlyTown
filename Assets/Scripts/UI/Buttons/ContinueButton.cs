using Dragger;
using SaveAndLoad;
using UI.Buttons;
using UnityEngine;

public class ContinueButton : AbstractButton
{
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Save _save;
    [SerializeField]private ItemsStorage _itemsStorage;
    
    protected override void OnClick()
    {
        _itemDragger.SetItem(_itemsStorage.SelectSaveItem,_itemsStorage.SelectSaveItem.ItemPosition);
        _itemDragger.SwitchOn();
    }
}
