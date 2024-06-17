using Enums;
using UI.Buttons;
using UnityEngine;

public class ItemButton : AbstractButton
{
    [SerializeField] private Items _itemName;
    [SerializeField]private ItemBuilder _itemBuilder;

    protected override void OnClick()
    {
        _itemBuilder.SetItems(_itemName);
    }
}
