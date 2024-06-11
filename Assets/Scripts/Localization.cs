using System;
using Agava.YandexGames;
using Enums;
using Lean.Localization;
using SaveAndLoad;
using UnityEngine;

public class Localization : MonoBehaviour
{
    private const string Language = "Language";
    private const string EnglishCode = "English";
    private const string RussianCode = "Russian";
    private const string TurkishCode = "Turkish";

    [SerializeField] private LeanLocalization _leanLocalization;
    [SerializeField] private Save _save;
    [SerializeField] private Load _load;

    private string _currentLanguage;
    private string _autoFoundLanguage;

    public event Action<string> LanguageChanged;

    private void Awake()
    {
        /*_currentLanguage = _load.Get(Language, RussianCode);
        ChangeLanguage(_currentLanguage);*/

#if UNITY_WEBGL&&!UNITY_EDITOR
        _autoFoundLanguage = YandexGamesSdk.Environment.i18n.lang;
        _currentLanguage = _load.Get(Language, _autoFoundLanguage);
        ChangeLanguage(_currentLanguage);
        LanguageChanged?.Invoke(_currentLanguage);
        Debug.Log("Автоязык определил " + _autoFoundLanguage);
#endif
    }

    private void Start()
    {
        LanguageChanged?.Invoke(_currentLanguage);
        Debug.Log("Язык устанавливаем " + _currentLanguage);
    }

    private void ChangeLanguage(string languageCode)
    {
        switch (languageCode)
        {
            case EnglishCode:
                _leanLocalization.SetCurrentLanguage(EnglishCode);
                break;

            case TurkishCode:
                _leanLocalization.SetCurrentLanguage(TurkishCode);
                break;

            case RussianCode:
                _leanLocalization.SetCurrentLanguage(RussianCode);
                break;
        }
    }

    public void SetLanguage(string languageCode)
    {
        _leanLocalization.SetCurrentLanguage(languageCode);
        _save.SetData(Language, languageCode);
    }
}