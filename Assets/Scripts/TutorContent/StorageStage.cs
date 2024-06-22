using System.Collections;
using Dragger;
using PossibilitiesContent;
using UnityEngine;

namespace TutorContent
{
    public class StorageStage : Stage
    {
        [SerializeField] private Storage _storage;
        [SerializeField] private GameObject _replaceButton;

        private void OnEnable()
        {
            // _itemDragger.PlaceChanged += ActivateMergeStage;
            // _merger.Mergered += ActivateStoragesStage;
            _storage.StoragePlaceChanged += ActivateReplaceStage;
        }

        private void OnDisable()
        {
            // _itemDragger.PlaceChanged -= ActivateMergeStage;
            // _merger.Mergered -= ActivateStoragesStage;
            _storage.StoragePlaceChanged -= ActivateReplaceStage;
        }


        public void ActivateReplaceStage()
        {
            // _replaceStage.SetActive(true);
            StartCoroutine(Active());
        }

        private IEnumerator Active()
        {
            yield return new WaitForSeconds(0.5f);
            HideItem();
            CloseCanvas();
            NextStage.gameObject.SetActive(true);
            NextStage.ShowDescription();
            // InputItemDragger.enabled = true;

            DescriptionStage.SetActive(false);

            _replaceButton.SetActive(true);

            gameObject.SetActive(false);
        }

        public override void OpenStage()
        {
        }
    }
}