using Dragger;
using UnityEngine;

namespace GoalContent
{
    public class GoalBuildItem : Goal
    {
        [SerializeField] private ItemThrower _itemThrower;

        private void OnEnable()
        {
            _itemThrower.BuildItem += OnChangeValue;
        }

        private void OnDisable()
        {
            _itemThrower.BuildItem -= OnChangeValue;
        }
    }
}