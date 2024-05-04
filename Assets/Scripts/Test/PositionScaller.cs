using System.Collections;
using UnityEngine;

public class PositionScaller : MonoBehaviour
{
    [SerializeField] private Vector3 _scale;
    [SerializeField] private float _duration;

    private float _elapsedTime;

    /*private void Start()
    {
        ScaleChanged();
    }*/

    public void ScaleChanged()
    {
        StartCoroutine(Scaling());
    }

    private IEnumerator Scaling()
    {
        _elapsedTime = 0;

        while (_elapsedTime < _duration)
        {
            float progress = _elapsedTime / _duration;
            transform.localScale = Vector3.Lerp(Vector3.zero, _scale, progress);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}