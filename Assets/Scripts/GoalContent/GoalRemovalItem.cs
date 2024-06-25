using PossibilitiesContent;
using UnityEngine;

namespace GoalContent
{
    public class GoalRemovalItem : Goal
    {
        [SerializeField] private RemovalItems _removalItems;

        private void OnEnable()
        {
            _removalItems.ItemRemoved += OnChangeValue;
        }

        private void OnDisable()
        {
            _removalItems.ItemRemoved -= OnChangeValue;
        }
    }
}