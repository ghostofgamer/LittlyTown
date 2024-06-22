using System.Collections;
using UnityEngine;

namespace TutorContent
{
    public class PlaceItemStage : Stage
    {
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);

        private void OnEnable()
        {
            ItemThrower.PlaceChanged += ActivateMergeStage;
        }

        private void OnDisable()
        {
            ItemThrower.PlaceChanged -= ActivateMergeStage;
        }

        private void ActivateMergeStage()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Activate());
        }

        private IEnumerator Activate()
        {
            yield return _waitForSeconds;
            HideItem();
            DescriptionGoalStage.SetActive(false);
            NextStage.gameObject.SetActive(true);
            NextStage.ShowDescription();
            gameObject.SetActive(false);
        }
    }
}