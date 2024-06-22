using Enums;
using UI.Buttons;
using UnityEngine;

namespace SandBoxContent
{
    public class ItemButton : AbstractButton
    {
        [SerializeField] private Items _itemName;
        [SerializeField]private ItemBuilder _itemBuilder;
        [SerializeField] private GameObject _closeContent;

        public Items ItemName=>_itemName;
    
        protected override void OnClick()
        {
            _itemBuilder.SetItems(_itemName);
        }

        public void BlockButton ()
        {
            Button.enabled = false;
            _closeContent.SetActive(true);
        }
    
        public void UnblockButton()
        {
            Button.enabled = true;
            _closeContent.SetActive(false);
        }
    }
}
