using CountersContent;
using InitializationContent;
using SaveAndLoad;
using UI.Buttons;
using UI.Screens;
using UnityEngine;

public class FinalTutorButton : AbstractButton
{
    private const string LastActiveMap = "LastActiveMap";
    private const string Map = "Map";
    private const string ActiveMap = "ActiveMap";
    
    [SerializeField] private GameLevelScreen _gameLevelScreen;
    [SerializeField] private GameObject[] _buttons;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private MoveCounter _moveCounter;
    [SerializeField] private Save _save;
    [SerializeField] private Initializator _initializator;
    [SerializeField]private TutorialScreen _tutorialScreen;
    
    private int _selectMap = 1;
    
    protected override void OnClick()
    {
        foreach (var button in _buttons)
            button.SetActive(true);

        _save.SetData(LastActiveMap, _selectMap);
        _save.SetData(Map, _initializator.Index);
        _save.SetData(ActiveMap + _initializator.Index, _selectMap);
        _tutorialScreen.Close();
        _gameLevelScreen.gameObject.SetActive(true);
        _gameLevelScreen.Open();
        _scoreCounter.enabled = true;
        _moveCounter.enabled = true;
    }
}