using MapsContent;
using SaveAndLoad;
using UnityEngine;

namespace InitializationContent
{
    public class Bootstrap : MonoBehaviour
    {
        private const string LastActiveMap = "LastActiveMap";
        private const string Map = "Map";

        [SerializeField] private Load _load;
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private ItemsStorage _itemsStorage;
        [SerializeField] private Initializator _initializator;
        [SerializeField] private ChooseMap _chooseMap;

        private int _startValue = 0;
        private int _valueLastMap;
        private int _valueCurrentMap;
        
        private void Start()
        {
            _valueLastMap = _load.Get(LastActiveMap, _startValue);
            _valueCurrentMap = _load.Get(Map, _startValue);

            _chooseMap.SetPosition(_valueCurrentMap);

            if (_valueLastMap == 0)
            {
                _initializator.SetIndex(_valueLastMap);
                _initializator.FillLists();
                _mapGenerator.GenerationWithoutSpawn(_initializator.Territories, _initializator.FinderPositions,
                    _initializator.ItemPositions, _initializator.CurrentMap.RoadsContainer,
                    _initializator.CurrentMap.StartItems);

                for (int i = 0; i < _initializator.AmountMaps; i++)
                {
                    if (i == _valueLastMap)
                        continue;

                    _mapGenerator.GenerateMap(i);
                }
            }
            else
            {
                _initializator.SetIndex(_valueCurrentMap);
                _initializator.FillLists();

                for (int i = 0; i < _initializator.AmountMaps; i++)
                    _mapGenerator.GenerateMap(i);

                // _itemsStorage.Load();
                // _itemsStorage.LoadDataInfo();
            }
        }
    }
}