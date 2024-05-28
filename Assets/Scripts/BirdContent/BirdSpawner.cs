using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] private BirdMovement[] _birdMovements;
    [SerializeField] private Transform[] _spawnPositions;

    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(5f);
    private int _indexBird;
    private int _indexPosition;
    private BirdMovement _currentBird;

    private void Start()
    {
        StartBird();
    }

    private void StartBird()
    {
        _indexBird = Random.Range(0, _birdMovements.Length);
        _indexPosition = Random.Range(0, _spawnPositions.Length);
        _birdMovements[_indexBird].transform.position = _spawnPositions[_indexPosition].position;
        _birdMovements[_indexBird].Init(_spawnPositions[_indexPosition]);
        _birdMovements[_indexBird].gameObject.SetActive(true);
        _birdMovements[_indexBird].BirdFinished += BirdDeactivation;
    }

    private void BirdDeactivation()
    {
        _birdMovements[_indexBird].BirdFinished -= BirdDeactivation;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CreateBird());
    }

    private IEnumerator CreateBird()
    {
        yield return _waitForSeconds;
        StartBird();
    }
}