using UnityEngine;

public class ButtonActivator : MonoBehaviour
{
    [SerializeField] private Initializator _initializator;
    [SerializeField] private ChooseMapButton[] _chooseMapButton;

    private void OnEnable()
    {
        _initializator.IndexChanged += ChangeActivityButton;
    }

    private void OnDisable()
    {
        _initializator.IndexChanged -= ChangeActivityButton;
    }

    private void ChangeActivityButton()
    {
        foreach (var button in _chooseMapButton)
        {
            button.gameObject.SetActive(button.Index != _initializator.Index);
        }
    }
}