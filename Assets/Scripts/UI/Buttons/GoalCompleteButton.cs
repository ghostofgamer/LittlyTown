using GoalContent;
using UnityEngine;

namespace UI.Buttons
{
    public class GoalCompleteButton : AbstractButton
    {
        [SerializeField] private GoalsCounter _goalsCounter;
        [SerializeField] private GameObject _progressInfo;
        [SerializeField] private GameObject _completeInfo;
        
        protected override void OnClick()
        {
            _goalsCounter.CompleteGoal();
            _progressInfo.SetActive(false);
            _completeInfo.SetActive(true);
        }
    }
}
