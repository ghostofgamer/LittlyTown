using System.Collections;
using UnityEngine;
using Wallets;

namespace UI.Buttons.RewardButtons
{
    public class RewardGoldButton : RewardButton
    {
        [SerializeField] private GoldWallet _goldWallet;
        [SerializeField] private GoldMovement[] _goldMovements;

        private int _maxGoldValue = 500;
        private int _minGoldValue = 160;
        private int _currentGoldValue;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.65f);
        private WaitForSeconds _waitForMoments = new WaitForSeconds(0.16f);
        private Coroutine _coroutine;

        protected override void ChangeRewardItem()
        {
            _currentGoldValue = Random.Range(_minGoldValue, _maxGoldValue);
            Debug.Log("Value    " + _currentGoldValue);

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