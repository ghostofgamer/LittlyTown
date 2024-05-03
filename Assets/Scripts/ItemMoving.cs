using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoving : MonoBehaviour
{
    [SerializeField] private Item _item;

    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
    private Vector3 _startPosition;

    private Coroutine _coroutineCyclically;

    public void MoveCyclically(Vector3 target)
    {
        if (!_item.IsActive)
            return;

        _startPosition = transform.position;

        if (_coroutineCyclically != null)
            StopCoroutine(_coroutineCyclically);

        _coroutineCyclically = StartCoroutine(Moving(target));
    }

    public void StopCoroutine()
    {
        if (_coroutineCyclically != null)
        {
            transform.position = _startPosition;
            StopCoroutine(_coroutineCyclically);
        }
    }

    private IEnumerator Moving(Vector3 targetItemPosition)
    {
        while (true)
        {
            Vector3 targetPosition = targetItemPosition;
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 target = transform.position + direction;

            float time = 0.0f;

            while (time < 1f)
            {
                float t = time / 1f;
                transform.position = Vector3.Lerp(_startPosition, target, t);
                time += Time.deltaTime;
                yield return null;
            }

            transform.position = _startPosition;
            yield return null;
        }
    }

    public void Move(Vector3 target)
    {
        _startPosition = transform.position;

        if (_coroutineCyclically != null)
            StopCoroutine(_coroutineCyclically);

        StartCoroutine(MoveTarget(target));
    }

    public IEnumerator MoveTarget(Vector3 targetItemPosition)
    {
        float time = 0.0f;

        while (time < 0.3f)
        {
            // Debug.Log("Move " + targetItemPosition);
            float t = time / 0.3f;
            transform.position = Vector3.Lerp(_startPosition, targetItemPosition, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetItemPosition;
        gameObject.SetActive(false);
    }
}