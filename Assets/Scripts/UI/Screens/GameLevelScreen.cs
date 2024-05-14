using UnityEngine;

namespace UI.Screens
{
    public class GameLevelScreen : AbstractScreen
    {
        private static readonly int OpenAnimation = Animator.StringToHash("Open");
        
        [SerializeField] private Animator _animator;

        public override void Open()
        {
            base.Open();
            _animator.SetTrigger(OpenAnimation);
        }
    }
}
