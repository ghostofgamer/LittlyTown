using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneDestroyer : MonoBehaviour
{
    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.15f);

    public void Destroy()
    {
        if(_coroutine!=null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(StartDestroy());
    }

    private IEnumerator StartDestroy()
    {
        yield return _waitForSeconds;
        gameObject.SetActive(false);
    }
}