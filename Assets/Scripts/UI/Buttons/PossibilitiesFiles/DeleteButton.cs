using System;
using PossibilitiesContent;
using UnityEngine;

namespace UI.Buttons.PossibilitiesFiles
{
    public class DeleteButton : AdditionalPossibilitiesButton
    {
        [SerializeField] private RemovalItems _removalItems;

        public event Action RemovalActivated;

        public event Action RemovalDeactivated;

        protected override void OnEnable()
        {
            base.OnEnable();
            _removalItems.Removed += OnDeactivation;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _removalItems.Removed -= OnDeactivation;
        }

        protected override void OnDeactivation()
        {
            base.OnDeactivation();
            RemovalDeactivated?.Invoke();
        }

        protected override void Activation()
        {
            base.Activation();
            RemovalActivated?.Invoke();
        }
    }
}