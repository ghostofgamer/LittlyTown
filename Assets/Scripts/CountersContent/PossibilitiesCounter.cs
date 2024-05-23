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
        [SerializeField] private int _startCount;
        [SerializeField] private Animator _animator;
        [SerializeField] private PossibilitieMovement _possibilitieMovement;
        
        private int _possibilitiesCount;

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
            _possibilitiesCount = _startCount;
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

        public void SetValue(int count)
        {
            _possibilitiesCount = count;
            Show();
        }

        public void SetCount()
        {
            _possibilitiesCount = _startCount;
            Show();
        }
    }
}