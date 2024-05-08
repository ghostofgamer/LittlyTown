using UnityEngine;

namespace UI.Buttons
{
    public class GoalButton : AbstractButton
    {
        [SerializeField] private GoalScreen _goalScreen;
        [SerializeField]private BlurScreen _blurScreen;
        
        protected override void OnClick()
        {
            _blurScreen.BlurActivation();
            _goalScreen.Open();
        }
    }
}
