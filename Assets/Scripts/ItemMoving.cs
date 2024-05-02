using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoving : MonoBehaviour
{
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);

    private Vector3 _startPosition;

    public void MoveCyclically(Vector3 target)
    {
        _startPosition = transform.position;
        StartCoroutine(Moving(target));
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
        StartCoroutine(MoveTarget(target));
    }

    public IEnumerator MoveTarget(Vector3 targetItemPosition)
    {
        float time = 0.0f;

        while (time < 1f)
        {
            float t = time / 1f;
            transform.position = Vector3.Lerp(_startPosition, targetItemPosition, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetItemPosition;
        // gameObject.SetActive(false);
    }
}