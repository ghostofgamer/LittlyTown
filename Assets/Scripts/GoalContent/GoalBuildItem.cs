using Dragger;
using UnityEngine;

namespace GoalContent
{
    public class GoalBuildItem : Goal
    {
        [SerializeField] private ItemThrower _itemThrower;

        private void OnEnable()
        {
            _itemThrower.BuildItem += ChangeValue;
        }

        private void OnDisable()
        {
            _itemThrower.BuildItem -= ChangeValue;
        }
    }
}