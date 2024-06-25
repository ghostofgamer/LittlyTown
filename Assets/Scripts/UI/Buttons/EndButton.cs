using InitializationContent;
using MapsContent;
using SaveAndLoad;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class EndButton : AbstractButton
    {
        private const string ActiveMap = "ActiveMap";
        private const string ItemStorageSave = "ItemStorageSave";

        [SerializeField] private Save _save;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private StartMap _startMap;
        [SerializeField] private ChooseMapScreen _chooseMapScreen;

        protected override void OnClick()
        {
            _save.SetData(ActiveMap + _initializator.Index, 0);

            if (PlayerPrefs.HasKey(ItemStorageSave + _initializator.Index))
                PlayerPrefs.DeleteKey(ItemStorageSave + _initializator.Index);

            _startMap.StartCreateWithoutSpawn();
            _chooseMapScreen.OnChangeActivationButton();
        }
    }
}