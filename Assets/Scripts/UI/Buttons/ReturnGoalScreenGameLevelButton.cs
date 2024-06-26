using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class ReturnGoalScreenGameLevelButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _goalsScreen;
        [SerializeField] private BlurScreen _blurScreen;

        protected override void OnClick()
        {
            _goalsScreen.Close();
            _blurScreen.BlurDeactivation();
        }
    }
}