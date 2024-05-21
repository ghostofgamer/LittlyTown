using Dragger;
using UI.Screens;
using UnityEngine;

public class ChooseMapScreen : AbstractScreen
{
    [SerializeField] private ItemDragger _itemDragger;

    public override void Open()
    {
        // _inputItemDragger.enabled = false;
        base.Open();
        _itemDragger.SwitchOff();
    }
}