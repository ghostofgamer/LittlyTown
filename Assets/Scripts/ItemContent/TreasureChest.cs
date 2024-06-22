using System.Collections;
using TMPro;
using UnityEngine;
using Wallets;
using Random = UnityEngine.Random;

namespace ItemContent
{
    public class TreasureChest : MonoBehaviour
    {
        [SerializeField] private Item _item;
        [SerializeField] private TMP_Text _rewardText;

        private GoldWallet _goldWallet;
        private int _reward;
        private int _minReward = 1000;
        private int _maxReward = 5000;
        private string _plus = "+ ";
        private WaitForSeconds _firstWaitForSeconds = new WaitForSeconds(0.365f);
        private WaitForSeconds _secondWaitForSeconds = new WaitForSeconds(0.1f);

        private void Update()
        {
            if (_rewardText.enabled)
            {
                _rewardText.transform.LookAt(Camera.main.transform.position);
                _rewardText.transform.rotation = Quaternion.LookRotation(-_rewardText.transform.forward);
            }
        }

        private void OnMouseUp()
        {
            StartCoroutine(OpenChest());
        }

        public void Init(GoldWallet goldWallet)
        {
            _goldWallet = goldWallet;
        }

        private IEnumerator OpenChest()
        {
            _reward = Random.Range(_minReward, _maxReward);
            _item.Deactivation();
            _goldWallet.SmoothlyIncreaseValue(_reward);
            _rewardText.enabled = true;
            _rewardText.text = _plus + _reward;
            yield return _firstWaitForSeconds;
            _item.ItemPosition.ClearingPosition();
            _rewardText.enabled = false;
            yield return _secondWaitForSeconds;
            gameObject.SetActive(false);
        }
    }
}