using UnityEngine;

namespace GoalContent
{
    public class GoalAnimation : AbstractAnimation
    {
        private static readonly int Show = Animator.StringToHash("Show");
        
        public void ShowGoal()
        {
            Animator.SetTrigger(Show);
        }
    }
}
