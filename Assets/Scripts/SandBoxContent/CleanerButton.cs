using UI.Buttons;
using UnityEngine;

namespace SandBoxContent
{
    public class CleanerButton : AbstractButton
    {
        [SerializeField] private ItemBuilder _itemBuilder;
        [SerializeField] private Cleaner _cleaner;
    
        protected override void OnClick()
        {
            _itemBuilder.enabled = false;
            _cleaner.enabled = true;
        }
    }
}
