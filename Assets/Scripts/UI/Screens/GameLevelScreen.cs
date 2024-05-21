using Dragger;
using UnityEngine;

namespace UI.Screens
{
    public class GameLevelScreen : AbstractScreen
    {
        private static readonly int OpenAnimation = Animator.StringToHash("Open");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private InputItemDragger _inputItemDragger;
        [SerializeField] private ItemDragger _itemDragger;

        public override void Open()
        {
            base.Open();
            _animator.SetTrigger(OpenAnimation);
            _inputItemDragger.enabled = true;
        }

        public override void Close()
        {
            base.Close();
            _inputItemDragger.enabled = false;
        }
    }
}
