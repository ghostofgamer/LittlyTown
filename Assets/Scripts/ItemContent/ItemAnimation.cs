using UnityEngine;

namespace ItemContent
{
    [RequireComponent(typeof(Item), typeof(Animator))]
    public class ItemAnimation : MonoBehaviour
    {
        private static readonly int Active = Animator.StringToHash("Active");
    
        private Item _item;
        private Animator _animator;

        private void Awake()
        {
            _item = GetComponent<Item>();
            _animator = GetComponent<Animator>();

            if (!_item.IsActive)
                _animator.SetBool(Active, false);
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
            _animator.SetBool(Active, true);
        }
    
        private void OnPlayAnimation()
        {
            _animator.SetBool(Active, false);
        }
    }
}