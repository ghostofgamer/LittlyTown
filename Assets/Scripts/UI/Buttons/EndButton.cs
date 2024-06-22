using InitializationContent;
using MapsContent;
using SaveAndLoad;
using UI.Buttons;
using UnityEngine;

public class EndButton : AbstractButton
{
    private const string ActiveMap = "ActiveMap";
    private const string ItemStorageSave = "ItemStorageSave";

    [SerializeField] private Save _save;
    [SerializeField] private Initializator _initializator;
    [SerializeField] private ChooseMap _chooseMap;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField]private StartMap _startMap;

    protected override void OnClick()
    {
        _save.SetData(ActiveMap + _initializator.Index, 0);

        if (PlayerPrefs.HasKey(ItemStorageSave + _initializator.Index))
        {
            PlayerPrefs.DeleteKey(ItemStorageSave + _initializator.Index);
            Debug.Log("Successfully deleted key: ");
        }

        // _initializator.SetIndex(0);
        // _startMap.StartCreate();
        _startMap.StartVisualCreate();

        // _chooseMap.ResetMapPosition();
        
        
        
        /*_mapGenerator.ShowTestFirstMap(_initializator.Territories, _initializator.FinderPositions,
            _initializator.ItemPositions, _initializator.CurrentMap.RoadsContainer,_initializator.CurrentMap.StartItems);*/
        
        // _mapGenerator.GenerationAllMap(_initializator.Index);
        
        // _initializator.SetIndex(_initializator.Index);
        // _chooseMap.SetPosition(0);
    }
}