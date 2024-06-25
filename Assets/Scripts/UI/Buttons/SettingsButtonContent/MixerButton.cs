using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.Buttons.SettingsButtonContent
{
    public class MixerButton : SettingsChangeButton
    {
        [SerializeField] private Image _toggleImage;
        [SerializeField] private AudioMixerGroup _outputGroup;

        protected override void ChangeValue()
        {
            _toggleImage.enabled = IsToggleOn;
            _outputGroup.audioMixer.SetFloat(SettingsModes.ToString(), IsToggleOn ? FirstValue : SecondValue);
            SaveValue();
        }
    }
}