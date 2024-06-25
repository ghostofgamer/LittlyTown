using InitializationContent;
using UI.Buttons;
using UnityEngine;

namespace MapsContent
{
    public class ButtonActivator : MonoBehaviour
    {
        [SerializeField] private Initializator _initializator;
        [SerializeField] private ChooseMapButton[] _chooseMapButton;
        [SerializeField] private GameObject[] _goldContentButtons;
        [SerializeField] private StartMap _startMap;

        private void OnEnable()
        {
            _initializator.IndexChanged += OnChangeActivityButton;
            _startMap.MapStarted += OnChangeActivityGoldContentButtons;
        }

        private void OnDisable()
        {
            _initializator.IndexChanged -= OnChangeActivityButton;
            _startMap.MapStarted += OnChangeActivityGoldContentButtons;
        }

        private void OnChangeActivityButton()
        {
            foreach (var button in _chooseMapButton)
                button.gameObject.SetActive(button.Index != _initializator.Index);
        }

        private void OnChangeActivityGoldContentButtons()
        {
            foreach (var button in _goldContentButtons)
                button.gameObject.SetActive(!_initializator.CurrentMap.IsMapWithoutProfit);
        }
    }
}