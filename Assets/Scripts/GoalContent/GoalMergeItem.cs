using UnityEngine;

namespace GoalContent
{
    public class GoalMergeItem : Goal
    {
        [SerializeField] private Merger _merge;

        private void OnEnable()
        {
            _merge.ItemMergered += ChangeValue;
        }

        private void OnDisable()
        {
            _merge.ItemMergered -= ChangeValue;
        }
    }
}
