using System.Collections;
using UnityEngine;

namespace BirdContent
{
    public class BirdSpawner : MonoBehaviour
    {
        private readonly WaitForSeconds WaitForSeconds = new WaitForSeconds(5f);

        [SerializeField] private BirdMovement[] _birdMovements;
        [SerializeField] private Transform[] _spawnPositions;

        private Coroutine _coroutine;
        private int _indexBird;
        private int _indexPosition;

        private void Start()
        {
            ActivateBird();
        }

        private void ActivateBird()
        {
            _indexBird = Random.Range(0, _birdMovements.Length);
            _indexPosition = Random.Range(0, _spawnPositions.Length);
            _birdMovements[_indexBird].transform.position = _spawnPositions[_indexPosition].position;
            _birdMovements[_indexBird].Init(_spawnPositions[_indexPosition]);
            _birdMovements[_indexBird].gameObject.SetActive(true);
            _birdMovements[_indexBird].BirdFinished += FinishBird;
        }

        private void FinishBird()
        {
            _birdMovements[_indexBird].BirdFinished -= FinishBird;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(CreateBird());
        }

        private IEnumerator CreateBird()
        {
            yield return WaitForSeconds;
            ActivateBird();
        }
    }
}