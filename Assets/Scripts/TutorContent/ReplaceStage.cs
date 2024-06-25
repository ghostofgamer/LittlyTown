using System.Collections;
using PossibilitiesContent;
using UnityEngine;

namespace TutorContent
{
    public class ReplaceStage : Stage
    {
        [SerializeField] private ReplacementPosition _replacementPosition;
        [SerializeField] private GameObject _removeButton;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);

        private void OnEnable()
        {
            _replacementPosition.PositionChanging += ActivateRemoveStage;
        }

        private void OnDisable()
        {
            _replacementPosition.PositionChanging -= ActivateRemoveStage;
        }

        private void ActivateRemoveStage()
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
            _removeButton.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}