using UnityEngine;

namespace UI.Buttons
{
    public class StorageButton : AbstractButton
    {
        [SerializeField]private Storage _storage;
        
        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            _storage.ChangeItem();
        }
    }
}
