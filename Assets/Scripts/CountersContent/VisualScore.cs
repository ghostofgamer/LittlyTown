using System;
using System.Collections;
using System.Collections.Generic;
using CountersContent;
using ItemContent;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VisualScore : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private Merger _merger;

    private Coroutine _coroutine;
    private Item _item;

    private void OnEnable()
    {
        _scoreCounter.ScoreIncomeChanged += Show;
        _merger.ItemMergered += SetItem;
    }

    private void OnDisable()
    {
        _scoreCounter.ScoreIncomeChanged -= Show;
        _merger.ItemMergered -= SetItem;
    }

    public void ScoreMove(int scoreValue, Vector3 position)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _scoreText.text = scoreValue.ToString();
        // StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return null;
    }

    private void Show(int score)
    {
        Debug.Log("Score Получается " + score);
        _item.RewardText.enabled = true;
        _item.RewardText.text = score.ToString();
    }

    private void SetItem(Item item)
    {
        _item = item;
    }
}