using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    [RequireComponent(typeof(Button), typeof(AudioSource))]
    public abstract class AbstractButton : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Button _button;

        protected Button Button => _button;

        protected AudioSource AudioSource => _audioSource;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _audioSource = GetComponent<AudioSource>();
        }

        protected abstract void OnClick();

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}