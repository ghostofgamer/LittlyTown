using System.Collections;
using CountersContent;
using SaveAndLoad;
using UI.Screens;
using UnityEngine;

public class TutorialScreen : AbstractScreen
{
    private const string LastActiveMap = "LastActiveMap";

    [SerializeField] private Load _load;
    [SerializeField] private GameObject[] _gameLevelButtons;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private MoveCounter _moveCounter;
    
    private int _defaultIndex = 0;
    private int _currentIndex;
    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(1.5f);
    private int _indexTutorialButton = 0;

    private bool _installFirstItem;
    private bool _mergeFirstTime;
    private bool _replaceFirstTime;
    private bool _bulldozerFirstTime;

    private void Start()
    {
        _currentIndex = _load.Get(LastActiveMap, _defaultIndex);

        if (_currentIndex == _defaultIndex)
        {
            _scoreCounter.enabled = false;
            _moveCounter.enabled = false;
             SlowOpen();
        }
        else
            enabled = false;
    }

    private void SlowOpen()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(StartOpen());
    }

    private IEnumerator StartOpen()
    {
        yield return _waitForSeconds;

        foreach (var button in _gameLevelButtons)
        {
            button.SetActive(false);
        }

        Open();
    }

    public void ChangeTutorialStage(GameObject closeContent, GameObject openContent)
    {
        openContent.SetActive(true);
        closeContent.SetActive(false);
    }
}