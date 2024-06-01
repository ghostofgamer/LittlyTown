using System.Collections;
using GoalContent;
using UnityEngine;

namespace UI.Screens
{
    public class GoalsScreen : AbstractScreen
    {
        [SerializeField] private GoalsCounter _goalsCounter;
        [SerializeField] private GameObject[] _buttons;

        private Coroutine _coroutine;
        private Coroutine _coroutineOffButtons;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);

        public override void Open()
        {
            base.Open();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ShowGoals());
        }

        public override void Close()
        {
            base.Close();

            if (_coroutineOffButtons != null)
                StopCoroutine(_coroutineOffButtons);

            _coroutineOffButtons = StartCoroutine(OffButtons());
        }

        private IEnumerator ShowGoals()
        {
            foreach (var goals in _goalsCounter.CurrentGoals)
            {
                goals.GetComponent<GoalAnimation>().ShowGoal();
                yield return _waitForSeconds;
            }
        }

        private IEnumerator OffButtons()
        {
            yield return _waitForSeconds;

            foreach (var button in _buttons)
                button.SetActive(false);
        }
    }
}