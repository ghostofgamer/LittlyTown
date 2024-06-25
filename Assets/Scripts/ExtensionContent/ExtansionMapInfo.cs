using SaveAndLoad;
using TMPro;
using UnityEngine;

namespace ExtensionContent
{
    public class ExtansionMapInfo : MonoBehaviour
    {
        private const string ExtensionTerritory = "ExtensionTerritory";

        [SerializeField] private string[] _informations;
        [SerializeField] private TMP_Text _parameterInfo;
        [SerializeField] private Load _load;
        [SerializeField] private int _index;

        private int _startValue = 0;
        private int _currentIndex;

        private void OnEnable()
        {
            LoadIndex();
        }

        private void Start()
        {
            _currentIndex = _load.Get(ExtensionTerritory + _index, _startValue);
            InitializationParameter();
        }

        private void InitializationParameter()
        {
            _parameterInfo.text = _informations[_currentIndex];
        }

        private void LoadIndex()
        {
            _currentIndex = _load.Get(ExtensionTerritory + _index, _startValue);
            InitializationParameter();
        }
    }
}