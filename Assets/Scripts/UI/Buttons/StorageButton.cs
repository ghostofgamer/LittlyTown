using UnityEngine;

namespace UI.Buttons
{
    public class StorageButton : AbstractButton
    {
        [SerializeField]private Storage _storage;
        
        protected override void OnClick()
        {
            _storage.ChangeItem();
        }
    }
}
