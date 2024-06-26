using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FlightScore : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rewardText;

        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
        private float _elapsedTime;
        private float _duration = 0.165f;
        private float _speed = 3f;
        private Vector3 _startPosition;
        private Vector3 _targetDirection;
        private Quaternion _targetRotation;
        private float _targetValue = 180;

        public void StartShow(Camera camera, int score)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ShowScore(camera, score));
        }

        private IEnumerator ShowScore(Camera camera, int score)
        {
            _startPosition = _rewardText.transform.position;
            _rewardText.enabled = true;
            _targetDirection = camera.transform.position - _rewardText.transform.position;
            _targetRotation = Quaternion.LookRotation(_targetDirection);
            _rewardText.transform.rotation = _targetRotation * Quaternion.Euler(0, _targetValue, 0);
            _rewardText.text = score.ToString();

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                _rewardText.transform.position += new Vector3(0, 1 * _speed * Time.deltaTime, 0);
                yield return null;
            }

            _rewardText.transform.position = _startPosition;
            _rewardText.enabled = false;
            yield return _waitForSeconds;
        }
    }
}