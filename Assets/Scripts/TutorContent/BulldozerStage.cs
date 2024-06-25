using System.Collections;
using PossibilitiesContent;
using UnityEngine;

namespace TutorContent
{
    public class BulldozerStage : Stage
    {
        [SerializeField] private RemovalItems _removalItems;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);

        private void OnEnable()
        {
            _removalItems.Removed += ActivatedFinalStage;
        }

        private void OnDisable()
        {
            _removalItems.Removed -= ActivatedFinalStage;
        }

        private void ActivatedFinalStage()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartActivate());
        }

        private IEnumerator StartActivate()
        {
            yield return _waitForSeconds;
            HideItem();
            CloseCanvas();
            DescriptionGoalStage.SetActive(false);
            NextStage.gameObject.SetActive(true);
            NextStage.ShowDescription();
            gameObject.SetActive(false);
        }
    }
}