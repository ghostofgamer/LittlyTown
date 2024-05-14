using System;
using PossibilitiesContent;
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
        [SerializeField] private PossibilitieMovement _possibilitieMovement;

        public int PossibilitiesCount => _possibilitiesCount;

        private void OnEnable()
        {
            _possibilitieMovement.MovementCompleted += IncreaseCount;
        }

        private void OnDisable()
        {
            _possibilitieMovement.MovementCompleted += IncreaseCount;
        }

        private void Start()
        {
            Show();
        }

        public void IncreaseCount(int value)
        {
            if (value < 0)
                return;

            _possibilitiesCount += value;
            Show();
            _animator.SetTrigger(Change);
        }

        public void DecreaseCount()
        {
            _possibilitiesCount--;
            Show();
            _animator.SetTrigger(Change);
        }

        private void Show()
        {
            _possibilitiesCountText.text = _possibilitiesCount.ToString();
        }
    }
}