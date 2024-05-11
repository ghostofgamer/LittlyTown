using System.Collections;
using GoalContent;
using UnityEngine;

namespace UI.Screens
{
    public class GoalsScreen : AbstractScreen
    {
        [SerializeField] private GoalsCounter _goalsCounter;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);

        public override void Open()
        {
            base.Open();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ShowGoals());
        }

        private IEnumerator ShowGoals()
        {
            foreach (var goals in _goalsCounter.CurrentGoals)
            {
                goals.GetComponent<GoalAnimation>().ShowGoal();
                yield return _waitForSeconds;
            }
        }
    }
}