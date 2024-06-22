using MergeContent;
using UnityEngine;

namespace TutorContent
{
    public class MergeStage : Stage
    {
        [SerializeField] private GameObject[] _storageButtons;
        [SerializeField] private Merger _merger;

        private void OnEnable()
        {
            _merger.Mergered += ActivateStoragesStage;
        }

        private void OnDisable()
        {
            _merger.Mergered -= ActivateStoragesStage;
        }

        private void ActivateStoragesStage()
        {
            foreach (var button in _storageButtons)
                button.SetActive(true);
            
            DescriptionGoalStage.SetActive(false);
            NextStage.gameObject.SetActive(true);
            NextStage.ShowDescription();
            InputItemDragger.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
