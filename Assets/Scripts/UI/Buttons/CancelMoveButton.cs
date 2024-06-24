using Keeper;
using UnityEngine;

namespace UI.Buttons
{
    public class CancelMoveButton : AbstractButton
    {
        [SerializeField] private MovesKeeper _movesKeeper;

        protected override void OnEnable()
        {
            base.OnEnable();
            _movesKeeper.StepChanged += SetInteractable;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _movesKeeper.StepChanged -= SetInteractable;
        }

        private void Start()
        {
            if (_movesKeeper.CurrentStep <= 0)
                Button.interactable = false;
        }

        private void SetInteractable(int currentStep)
        {
            Button.interactable = currentStep > 0;
        }
    
        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            _movesKeeper.CancelLastStep();
        }
    }
}
