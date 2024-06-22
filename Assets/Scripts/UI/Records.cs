using InitializationContent;
using SaveAndLoad;
using TMPro;
using UnityEngine;

public class Records : MonoBehaviour
{
    private const string MaxRecord = "MaxRecord";

    [SerializeField] private GameObject[] _recordsContent;
    [SerializeField] private TMP_Text[] _recordsInfo;
    [SerializeField] private Load _load;
    [SerializeField] private Initializator _initializator;

    private void OnEnable()
    {
        _initializator.IndexChanged += Show;
    }

    private void OnDisable()
    {
        _initializator.IndexChanged -= Show;
    }

    public void Show()
    {
        int score = _load.Get(MaxRecord + _initializator.Index, 0);

        
        if (score > 0)
        {
            _recordsContent[_initializator.Index].SetActive(true);
            _recordsInfo[_initializator.Index].text = score.ToString();
        }
        else
        {
            _recordsContent[_initializator.Index].SetActive(false);
        }
    }
}