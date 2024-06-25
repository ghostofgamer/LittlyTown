using PossibilitiesContent;
using TMPro;
using UnityEngine;

namespace CountersContent
{
    public class PossibilitiesCounter : MonoBehaviour
    {
        private static readonly int Change = Animator.StringToHash("Change");

        [SerializeField] private TMP_Text _possibilitiesCountText;
        [SerializeField] private int _startCount;
        [SerializeField] private Animator _animator;
        [SerializeField] private MovementIcon _movementIcon;

        public int PossibilitiesCount { get; private set; }

        private void OnEnable()
        {
            _movementIcon.MovementCompleted += OnIncreaseCount;
        }

        private void OnDisable()
        {
            _movementIcon.MovementCompleted -= OnIncreaseCount;
        }

        private void Start()
        {
            PossibilitiesCount = _startCount;
            Show();
        }

        public void OnIncreaseCount(int value)
        {
            if (value < 0)
                return;

            PossibilitiesCount += value;
            Show();
            _animator.SetTrigger(Change);
        }

        public void DecreaseCount()
        {
            PossibilitiesCount--;
            Show();
            _animator.SetTrigger(Change);
        }

        public void SetValue(int count)
        {
            PossibilitiesCount = count;
            Show();
        }

        public void ResetCount()
        {
            PossibilitiesCount = _startCount;
            Show();
        }

        private void Show()
        {
            _possibilitiesCountText.text = PossibilitiesCount.ToString();
        }
    }
}