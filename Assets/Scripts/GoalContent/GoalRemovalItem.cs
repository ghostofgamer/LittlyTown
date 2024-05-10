using UnityEngine;

namespace GoalContent
{
    public class GoalRemovalItem : Goal
    {
        [SerializeField]private RemovalItems _removalItems;

        private void OnEnable()
        {
            _removalItems.ItemRemoved += ChangeValue;
        }

        private void OnDisable()
        {
            _removalItems.ItemRemoved -= ChangeValue;
        }
    }
}
