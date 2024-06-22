using System.Collections;
using System.Collections.Generic;
using InitializationContent;
using MapsContent;
using SaveAndLoad;
using TutorContent;
using UnityEngine;

public class RestartTutorial : MonoBehaviour
{
    private const string LastActiveMap = "LastActiveMap";
    [SerializeField] private ChooseMap _chooseMap;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField]private Transform _environment;
    [SerializeField] private Map[] _maps;
    [SerializeField] private Save _save;
    [SerializeField]private Initializator _initializator;
    [SerializeField] private Bootstrap _bootstrap;
    [SerializeField]private TutorialScreen _tutorialScreen;
    [SerializeField] private StartMap _startMap;
    [SerializeField] private GameObject[] _stages;
    [SerializeField] private GameObject[] _firstStageContent;
    [SerializeField] private MapGenerator _mapGenerator;
    
    private int _startValue = 0;

    public void StartTutorial()
    {
        _chooseMap.enabled = true;
        _chooseMap.ResetMapPosition();
        _chooseMap.enabled = false;
        _initializator.ResetIndex();
        _initializator.FillLists();
        // _chooseMap.SetPosition(0);
        // _environment.position = _targetPosition;
        _save.SetData("Map", 0);
        _save.SetData(LastActiveMap, _startValue);
        
        for (int i = 0; i < _maps.Length; i++)
        {
            if (i == 0)
            {
                _maps[i].gameObject.SetActive(true);
                continue;
            }

            _maps[i].gameObject.SetActive(false);
        }

        foreach (var stage in _stages)
        {
            stage.SetActive(false);
        }
        foreach (var stage in _firstStageContent)
        {
            stage.SetActive(true);
        }
        
        _mapGenerator.ShowTestFirstMap(_initializator.Territories, _initializator.FinderPositions,
            _initializator.ItemPositions, _initializator.CurrentMap.RoadsContainer,_initializator.CurrentMap.StartItems);
        
        // _bootstrap.FirstGenerate();
        
        _tutorialScreen.StartTutorial();
        _startMap.NEWStartVisualCreate();
    }
}
