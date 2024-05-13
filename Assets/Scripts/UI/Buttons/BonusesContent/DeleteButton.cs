using System;
using UnityEngine;

namespace UI.Buttons.BonusesContent
{
    public class DeleteButton : AdditionalFeaturesButton
    {
        [SerializeField] private RemovalItems _removalItems;
        
        public event Action RemovalActivated;

        public event Action RemovalDeactivated;

        protected override void OnEnable()
        {
            base.OnEnable();
            _removalItems.Removed += Deactivation;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _removalItems.Removed -= Deactivation;
        }

        protected override void Deactivation()
        {
            base.Deactivation();
            RemovalDeactivated?.Invoke();
        }

        protected override void Activation()
        {
            base.Activation();
            RemovalActivated?.Invoke();
        }
    }
}
