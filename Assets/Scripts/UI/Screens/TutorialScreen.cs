using System.Collections;
using System.Collections.Generic;
using CameraContent;
using CountersContent;
using EnvironmentContent;
using ItemPositionContent;
using SaveAndLoad;
using UnityEngine;

namespace UI.Screens
{
    public class TutorialScreen : AbstractScreen
    {
        private const string LastActiveMap = "LastActiveMap";

        [SerializeField] private Load _load;
        [SerializeField] private GameObject[] _gameLevelButtons;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private VisualItemsDeactivator _visualItemsDeactivator;
        [SerializeField] private List<ItemPosition> _itemPositions = new List<ItemPosition>();
        [SerializeField] private TurnEnvironment _turnEnvironment;
        [SerializeField] private GameObject _environment;
        [SerializeField] private ShopItems _shopItems;
        [SerializeField] private CameraScrolling _cameraScrolling;

        private int _defaultIndex = 0;
        private int _currentIndex;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(1.5f);

        private void Start()
        {
            _currentIndex = _load.Get(LastActiveMap, _defaultIndex);

            if (_currentIndex == _defaultIndex)
                StartTutorial();
            else
                enabled = false;
        }

        public void StartTutorial()
        {
            _cameraScrolling.enabled = false;
            _visualItemsDeactivator.SetPositions(_itemPositions);
            _turnEnvironment.SetEnvironment(_environment);
            _scoreCounter.enabled = false;
            _moveCounter.enabled = false;
            SlowOpen();
        }

        public void ChangeTutorialStage(GameObject closeContent, GameObject openContent)
        {
            openContent.SetActive(true);
            closeContent.SetActive(false);
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
                button.SetActive(false);

            _shopItems.SetStartPrice();
            OnOpen();
        }
    }
}