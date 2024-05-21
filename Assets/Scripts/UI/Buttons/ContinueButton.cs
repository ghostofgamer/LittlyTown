using Dragger;
using SaveAndLoad;
using UI.Buttons;
using UnityEngine;

public class ContinueButton : AbstractButton
{
    [SerializeField] private ItemDragger _itemDragger;
    [SerializeField] private Save _save;
    
    protected override void OnClick()
    {
        _itemDragger.SwitchOn();
    }
}
