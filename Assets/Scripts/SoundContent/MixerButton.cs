using Enums;
using UI.Buttons;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerButton : AbstractButton
{
    [SerializeField] private Image _toggleImage;
    [SerializeField] private AudioMixerGroup _outputGroup;
    [SerializeField] private SoundMixer _soundMixer;

    private float _volumeOn = 0;
    private float _volumeOff = -80;
    private bool _isToggleOn;
    
    protected override void OnClick()
    {
        _isToggleOn = !_isToggleOn;

        _toggleImage.enabled = _isToggleOn;

        if (_isToggleOn)
            _outputGroup.audioMixer.SetFloat(_soundMixer.ToString(), _volumeOn);
        else
            _outputGroup.audioMixer.SetFloat(_soundMixer.ToString(), _volumeOff);
    }
}

