using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PostProcessContent
{
    public abstract class PostProcessChanger : MonoBehaviour
    {
        [SerializeField] private PostProcessVolume _postProcessVolume;
        [SerializeField]private float _defaultValue;
        [SerializeField] private float _targetValue;
        
        private float _elapsedTime;
        private float _duration = 0.5f;
        private Coroutine _coroutine;
        
        protected PostProcessVolume PostProcessVolume => _postProcessVolume;

        protected float DefaultValue => _defaultValue;

        protected float CurrentValue { get; private set; }

        protected abstract void ChangeValue();

        protected void SetValue(float value)
        {
            CurrentValue = value;
        }

        public virtual void TurnOn()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ChangeValue(CurrentValue, _targetValue));
        }

        public virtual void TurnOff()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ChangeValue(CurrentValue, _defaultValue));
        }

        private IEnumerator ChangeValue(float start, float end)
        {
            _elapsedTime = 0;

            while (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
                CurrentValue = Mathf.Lerp(start, end, _elapsedTime / _duration);
                ChangeValue();
                yield return null;
            }

            CurrentValue = end;
            ChangeValue();
        }
    }
}
