using Enums;
using SaveAndLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.SettingsButtonContent
{
    public class ChangeLocalizationButton : AbstractButton
    {
        [SerializeField] private Localization _localization;
        [SerializeField] private string _language;
        [SerializeField] private Image[] _selectedImages;
        [SerializeField] private Image _imageSelected;

        protected override void OnEnable()
        {
            base.OnEnable();
            _localization.LanguageChanged += SetLanguage;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _localization.LanguageChanged += SetLanguage;
        }

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            ChangeLanguage();
        }

        private void ChangeLanguage()
        {
            foreach (var image in _selectedImages)
                image.gameObject.SetActive(false);

            _localization.SetLanguage(_language);
            _imageSelected.gameObject.SetActive(true);
        }

        private void SetLanguage(string language)
        {
            if (language == _language)
                ChangeLanguage();
        }
    }
}