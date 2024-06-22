using Dragger;
using UnityEngine;

namespace GoalContent
{
    public class GoalBuildItem : Goal
    {
        [SerializeField] private ItemDragger _itemDragger;
        [SerializeField] private ItemThrower _itemThrower;
        
        private void OnEnable()
        {
            // _itemDragger.BuildItem += ChangeValue;
            _itemThrower.BuildItem += ChangeValue;
        }

        private void OnDisable()
        {
            // _itemDragger.BuildItem -= ChangeValue;
            _itemThrower.BuildItem -= ChangeValue;
        }
    }
}