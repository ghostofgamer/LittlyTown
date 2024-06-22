using System.Collections;
using Dragger;
using MergeContent;
using UnityEngine;

namespace TutorContent
{
    public class PlaceItemStage : Stage
    {
        [SerializeField] private Merger _merger;
        
        private void OnEnable()
        {
            // ItemDragger.PlaceChanged += ActivateMergeStage;
            ItemThrower.PlaceChanged += ActivateMergeStage;
            // _merger.Mergered += ActivateMergeStage;
        }

        private void OnDisable()
        {
            // ItemDragger.PlaceChanged -= ActivateMergeStage;
            ItemThrower.PlaceChanged -= ActivateMergeStage;
            // _merger.Mergered -= ActivateMergeStage;
        }

        private void ActivateMergeStage()
        {
            StartCoroutine(Activate());
        }

        public override void OpenStage()
        {
        }

        private IEnumerator Activate()
        {
            yield return new WaitForSeconds(0.5f);
            HideItem();
            DescriptionGoalStage.SetActive(false);
            NextStage.gameObject.SetActive(true);
            NextStage.ShowDescription();
            gameObject.SetActive(false);
        }
    }
}