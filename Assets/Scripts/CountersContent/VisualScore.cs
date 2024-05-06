using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VisualScore : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private Coroutine _coroutine;

    
    public void ScoreMove(int scoreValue,Vector3 position)
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
}