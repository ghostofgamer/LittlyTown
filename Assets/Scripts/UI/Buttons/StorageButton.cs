using PossibilitiesContent;
using UnityEngine;

namespace UI.Buttons
{
    public class StorageButton : AbstractButton
    {
        [SerializeField] private Storage _storage;
        [SerializeField] private GameObject _closeIcon;

        public void OpenStorage()
        {
            Button.enabled = true;
            _closeIcon.SetActive(false);
        }

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            _storage.ChangeItem();
        }
    }
}