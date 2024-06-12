using TutorContent;
using UI.Buttons;
using UnityEngine;

public class StartStageButton : AbstractButton
{
    [SerializeField] private Stage _oldStage;
    [SerializeField] private Stage _targetStage;
    [SerializeField] private Stage _currentStage;
    
    protected override void OnClick()
    {
        _currentStage.ShowItem();
        _currentStage.OpenCanvas();
        /*if (_targetStage != null)
            _targetStage.gameObject.SetActive(true);

        if (_oldStage != null)
            _oldStage.gameObject.SetActive(false);*/
    }
}
