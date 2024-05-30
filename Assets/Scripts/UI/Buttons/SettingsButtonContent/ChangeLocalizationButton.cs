using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.SettingsButtonContent
{
    public class ChangeLocalizationButton : AbstractButton
    {
        [SerializeField] private Languages _languages;
        [SerializeField] private Localization _localization;
        [SerializeField] private string _language;
        [SerializeField] private Image[] _selectedImages;
        [SerializeField] private Image _imageSelected;

        protected override void OnEnable()
        {
            base.OnEnable();
            _localization.LanguageChanged += Check;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _localization.LanguageChanged += Check;
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

        private void Check(string language)
        {
            if (language == _language)
            {
                Debug.Log("попал ");
                ChangeLanguage();
            }
        }
    }
}