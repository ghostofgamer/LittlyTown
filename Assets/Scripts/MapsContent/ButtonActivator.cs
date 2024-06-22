using InitializationContent;
using UnityEngine;

namespace MapsContent
{
    public class ButtonActivator : MonoBehaviour
    {
        [SerializeField] private Initializator _initializator;
        [SerializeField] private ChooseMapButton[] _chooseMapButton;
        [SerializeField] private GameObject[] _goldContentButtons;
        [SerializeField]private StartMap _startMap;

        private void OnEnable()
        {
            _initializator.IndexChanged += ChangeActivityButton;
            _startMap.MapStarted += ChangeActivityGoldContentButtons;
        }

        private void OnDisable()
        {
            _initializator.IndexChanged -= ChangeActivityButton;
            _startMap.MapStarted += ChangeActivityGoldContentButtons;
        }

        private void ChangeActivityButton()
        {
            foreach (var button in _chooseMapButton)
                button.gameObject.SetActive(button.Index != _initializator.Index);
        }

        private void ChangeActivityGoldContentButtons()
        {
            foreach (var button in _goldContentButtons)
                button.gameObject.SetActive(!_initializator.CurrentMap.IsMapWithoutProfit);
        }
    }
}