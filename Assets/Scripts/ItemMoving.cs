using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoving : MonoBehaviour
{
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);

    private Vector3 _startPosition;

    public void Move(Vector3 target)
    {
        _startPosition = transform.position;
        StartCoroutine(Moving(target));
    }

    private IEnumerator Moving(Vector3 targetItemPosition)
    {
        while (true)
        {
            Vector3 targetPosition = targetItemPosition;

            // Вычислить направление к цели
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Вычислить конечную позицию (на один шаг в направлении цели)
            Vector3 target = transform.position + direction;

            float time = 0.0f;

            while (time < 1f)
            {
                float t = time / 1f;
                transform.position = Vector3.Lerp(_startPosition, target, t);
                time += Time.deltaTime;
                yield return null;
            }

            Debug.Log("versi");
            transform.position = _startPosition;
            // yield return _waitForSeconds;
            yield return null;

            /*float time = 0;
            Vector3 targetPosition = targetItemPosition;
            Vector3 direction = transform.position - targetPosition;
            direction.Normalize();
            Vector3 target = transform.position -= direction;

            while (time < 1)
            {
                float t = time / 1;
                Debug.Log("Direction" + direction);
                transform.position = Vector3.Lerp(transform.position, target, t);
                time += Time.deltaTime;
                yield return null;
            }

            transform.position = _startPosition;
            yield return _waitForSeconds;*/
        }
    }
}