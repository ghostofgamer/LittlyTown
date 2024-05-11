using UnityEngine;

namespace ItemContent
{
    [RequireComponent(typeof(Item), typeof(Animator))]
    public class ItemAnimation : AbstractAnimation
    {
        private static readonly int Active = Animator.StringToHash("Active");
        private static readonly int Busy = Animator.StringToHash("Busy");
        private static readonly int Positioning = Animator.StringToHash("Positioning");
    
        private Item _item;

        protected override void Awake()
        {
            base.Awake();
            _item = GetComponent<Item>();

            if (!_item.IsActive)
                Animator.SetBool(Active, false);

        }

        private void OnEnable()
        {
            _item.Activated += OnStopAnimation;
            _item.Deactivated += OnPlayAnimation;
        }

        private void OnDisable()
        {
            _item.Activated -= OnStopAnimation;
            _item.Deactivated -= OnPlayAnimation;
        }

        private void OnStopAnimation()
        {
            Animator.SetBool(Active, true);
        }
    
        private void OnPlayAnimation()
        {
            Animator.SetBool(Active, false);
        }

        public void BusyPositionAnimation()
        {
            Animator.SetTrigger(Busy);
        }

        public void PositioningAnimation()
        {
            Animator.SetTrigger(Positioning); 
        }
    }
}