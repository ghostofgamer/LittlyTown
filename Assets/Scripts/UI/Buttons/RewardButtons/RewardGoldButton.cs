using System.Collections;
using PossibilitiesContent;
using TMPro;
using UnityEngine;
using Wallets;

namespace UI.Buttons.RewardButtons
{
    public class RewardGoldButton : RewardButton
    {
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private GoldMovement[] _goldMovements;
        [SerializeField] private TMP_Text _goldText;
        
        private int _maxGoldValue = 500;
        private int _minGoldValue = 160;
        private int _currentGoldValue;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.65f);
        private WaitForSeconds _waitForMoments = new WaitForSeconds(0.16f);
        private Coroutine _coroutine;

        public void DetermineGoldAmount()
        {
            _currentGoldValue = Random.Range(_minGoldValue, _maxGoldValue);
            _goldText.text = _currentGoldValue.ToString();
        }
        
        protected override void ChangeRewardItem()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ActivateSelection());
        }

        private IEnumerator ActivateSelection()
        {
            yield return _waitForSeconds;

            foreach (GoldMovement goldMovement in _goldMovements)
            {
                yield return _waitForMoments;
                goldMovement.StartMove();
            }

            yield return _waitForSeconds;
            _goldWallet.SmoothlyIncreaseValue(_currentGoldValue);
        }
    }
}