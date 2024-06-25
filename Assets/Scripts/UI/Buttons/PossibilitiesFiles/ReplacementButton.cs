using System;
using PossibilitiesContent;
using UnityEngine;

namespace UI.Buttons.PossibilitiesFiles
{
    public class ReplacementButton : AdditionalPossibilitiesButton
    {
        [SerializeField] private ReplacementPosition _replacementPosition;

        public event Action ReplaceActivated;

        public event Action ReplaceDeactivated;

        protected override void OnEnable()
        {
            base.OnEnable();
            _replacementPosition.PositionsChanged += OnDeactivation;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _replacementPosition.PositionsChanged -= OnDeactivation;
        }

        protected override void OnDeactivation()
        {
            base.OnDeactivation();
            ReplaceDeactivated?.Invoke();
        }

        protected override void Activation()
        {
            base.Activation();
            ReplaceActivated?.Invoke();
        }
    }
}