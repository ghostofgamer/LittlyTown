using System.Collections;
using Dragger;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GoalScreen : MonoBehaviour
{
    [SerializeField] private InputItemDragger _itemDragger;
    
    private CanvasGroup _canvasGroup;
    private float _elapsedtime; 
    private float _duration=1f;

    private Coroutine _coroutine;
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void Open()
    {
        if(_coroutine!=null)
            StopCoroutine(_coroutine);
        
        _coroutine=StartCoroutine(Fade(0,1));
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _itemDragger.enabled = false;
    }

    public void Close()
    {
        if(_coroutine!=null)
            StopCoroutine(_coroutine);
        
        _coroutine=StartCoroutine(Fade(1,0));
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _itemDragger.enabled = true;
    }

    private IEnumerator Fade(float start, float end)
    {
        _elapsedtime = 0f;

        while (_elapsedtime < _duration)
        {
            _canvasGroup.alpha = Mathf.Lerp(start, end, _elapsedtime / _duration);
            _elapsedtime += Time.deltaTime;
           yield return null; 
        }

        _canvasGroup.alpha = end;
    }
}