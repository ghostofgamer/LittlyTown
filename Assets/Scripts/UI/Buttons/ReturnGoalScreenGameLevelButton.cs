using UnityEngine;

namespace UI.Buttons
{
    public class ReturnGoalScreenGameLevelButton : AbstractButton
    {
        [SerializeField] private GoalScreen _goalscreen;
        [SerializeField]private BlurScreen _blurScreen;
        
        protected override void OnClick()
        {
            _goalscreen.Close();
            _blurScreen.BlurDeactivation();
        }
    }
}
