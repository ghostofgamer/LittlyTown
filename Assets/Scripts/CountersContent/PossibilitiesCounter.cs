using TMPro;
using UnityEngine;

namespace CountersContent
{
    public class PossibilitiesCounter : MonoBehaviour
    {
        private static readonly int Change = Animator.StringToHash("Change");
        
        [SerializeField] private TMP_Text _possibilitiesCountText;
        [SerializeField] private int _possibilitiesCount;
        [SerializeField] private Animator _animator;
        
        public int PossibilitiesCount => _possibilitiesCount;

        private void Start()
        {
            Show();
        }

        public void AddFeature(int value)
        {
            if (value < 0)
                return;

            _possibilitiesCount += value;
            Show();
            _animator.SetTrigger(Change);
        }

        public void RemoveFeature()
        {
            _possibilitiesCount --;
            Show();
            _animator.SetTrigger(Change);
        }

        private void Show()
        {
            _possibilitiesCountText.text = _possibilitiesCount.ToString();
        }
    }
}
