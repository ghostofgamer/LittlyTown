using UnityEngine;

namespace UI.Buttons
{
    public class ReturnGoalScreenGameLevelButton : AbstractButton
    {
        [SerializeField] private GoalScreen _goalScreen;
        [SerializeField]private BlurScreen _blurScreen;
        
        protected override void OnClick()
        {
            _goalScreen.Close();
            _blurScreen.BlurDeactivation();
        }
    }
}
