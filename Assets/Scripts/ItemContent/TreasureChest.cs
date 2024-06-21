using System;
using System.Collections;
using ItemContent;
using TMPro;
using UnityEngine;
using Wallets;
using Random = UnityEngine.Random;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private TMP_Text _rewardText;

    private GoldWallet _goldWallet;
    private int _reward;
    private int _minReward = 1000;
    private int _maxReward = 5000;

    private void OnMouseUp()
    {
        StartCoroutine(OpenChest());
    }

    private IEnumerator OpenChest()
    {
        _reward = Random.Range(_minReward, _maxReward);
        Debug.Log("даю денег");
        _item.Deactivation();
        Debug.Log(_item.name);
        _goldWallet.SmoothlyIncreaseValue(_reward);
        _rewardText.enabled = true;
        _rewardText.text = "+ " + _reward;
        yield return new WaitForSeconds(0.365f);
        _item.ItemPosition.ClearingPosition();
        _rewardText.enabled = false;
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_rewardText.enabled)
        {
            _rewardText.transform.LookAt(Camera.main.transform.position);
            _rewardText.transform.rotation = Quaternion.LookRotation(-_rewardText.transform.forward);
            // _rewardText.transform.Rotate(0, _rewardText.transform.eulerAngles.y,  _rewardText.transform.eulerAngles.z);
        }
    }

    public void Init(GoldWallet goldWallet)
    {
        _goldWallet = goldWallet;
    }
}