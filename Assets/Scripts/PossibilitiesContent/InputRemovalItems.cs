using UnityEngine;

namespace PossibilitiesContent
{
    public class InputRemovalItems : MonoBehaviour
    {
        [SerializeField] private RemovalItems _removalItems;
        [SerializeField] private AnimationRemovalItem _animationRemovalItem;

        private bool _isLooking;

        private void Update()
        {
            if (!_removalItems.IsWorking)
                return;

            if (Input.GetMouseButtonDown(0))
                _isLooking = true;

            if (_isLooking)
                _animationRemovalItem.ActivateAnimation();

            if (Input.GetMouseButtonUp(0))
            {
                _isLooking = false;
                _removalItems.StartRemove();
            }
        }
    }
}