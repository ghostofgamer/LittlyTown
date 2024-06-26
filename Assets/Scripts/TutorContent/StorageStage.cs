using System.Collections;
using PossibilitiesContent;
using UnityEngine;

namespace TutorContent
{
    public class StorageStage : Stage
    {
        [SerializeField] private Storage _storage;
        [SerializeField] private GameObject _replaceButton;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);

        private void OnEnable()
        {
            _storage.StoragePlaceChanged += OnActivateReplaceStage;
        }

        private void OnDisable()
        {
            _storage.StoragePlaceChanged -= OnActivateReplaceStage;
        }

        private void OnActivateReplaceStage()
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
            NextStage.gameObject.SetActive(true);
            NextStage.ShowDescription();
            DescriptionStage.SetActive(false);
            _replaceButton.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}