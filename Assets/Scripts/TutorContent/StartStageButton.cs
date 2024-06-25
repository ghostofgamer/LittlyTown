using UI.Buttons;
using UnityEngine;

namespace TutorContent
{
    public class StartStageButton : AbstractButton
    {
        [SerializeField] private Stage _currentStage;

        protected override void OnClick()
        {
            _currentStage.ShowItem();
            _currentStage.OpenCanvas();
        }
    }
}