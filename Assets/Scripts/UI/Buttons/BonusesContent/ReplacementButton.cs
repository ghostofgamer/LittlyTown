using System;
using UnityEngine;

namespace UI.Buttons.BonusesContent
{
    public class ReplacementButton : AdditionalFeaturesButton
    {
        [SerializeField] private ReplacementPosition _replacementPosition;
        
        public event Action ReplaceActivated;
        
        public event Action ReplaceDeactivated;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _replacementPosition.PositionsChanged += Deactivation;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _replacementPosition.PositionsChanged -= Deactivation;
        }

        protected override void Deactivation()
        {
            base.Deactivation();
            ReplaceDeactivated?.Invoke();
        }

        protected override void Activation()
        {
            base.Activation();
            ReplaceActivated?.Invoke();
        }
    }
}
