using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMovement : MonoBehaviour
{
    [SerializeField] private RectTransform _targetWallet;

    private float _elapsedTime;
    private float _duration = 0.5f;
    private Vector3 _targetPosition;
    private Vector3 _startPosition = Vector3.zero;
    private Coroutine _coroutine;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _targetPosition = _targetWallet.localPosition;
        Debug.Log("StartTarget " + _targetPosition);
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    public void StartMove()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeAnimation());
    }

    private IEnumerator ChangeAnimation()
    {
        _elapsedTime = 0f;
        _canvasGroup.alpha = 1;
        _startPosition = Vector3.zero;

        while (_elapsedTime < _duration)
        {
            _elapsedTime += Time.deltaTime;
            Debug.Log("Coroutine " + _targetPosition);
            gameObject.transform.localPosition =
                Vector3.Lerp(_startPosition, _targetPosition, _elapsedTime / _duration);
            yield return null;
        }

        _canvasGroup.alpha = 0;
    }
}