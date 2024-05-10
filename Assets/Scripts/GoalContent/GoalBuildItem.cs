using Dragger;
using UnityEngine;

namespace GoalContent
{
    public class GoalBuildItem : Goal
    {
        [SerializeField] private ItemDragger _itemDragger;

        private void OnEnable()
        {
            _itemDragger.BuildItem += ChangeValue;
        }

        private void OnDisable()
        {
            _itemDragger.BuildItem -= ChangeValue;
        }
    }
}