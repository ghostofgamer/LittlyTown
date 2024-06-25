using InitializationContent;
using SaveAndLoad;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Records : MonoBehaviour
    {
        private const string MaxRecord = "MaxRecord";

        [SerializeField] private GameObject[] _recordsContent;
        [SerializeField] private TMP_Text[] _recordsInfo;
        [SerializeField] private Load _load;
        [SerializeField] private Initializator _initializator;

        private int _score;

        private void OnEnable()
        {
            _initializator.IndexChanged += OnShow;
        }

        private void OnDisable()
        {
            _initializator.IndexChanged -= OnShow;
        }

        public void OnShow()
        {
            _score = _load.Get(MaxRecord + _initializator.Index, 0);

            if (_score > 0)
            {
                _recordsContent[_initializator.Index].SetActive(true);
                _recordsInfo[_initializator.Index].text = _score.ToString();
            }
            else
            {
                _recordsContent[_initializator.Index].SetActive(false);
            }
        }
    }
}