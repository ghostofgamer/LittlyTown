using UnityEngine;

namespace DayNightContent
{
    public class BackGroundMusic : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private DayNight _dayNight;
        [SerializeField] private AudioClip _dayAudioClip;
        [SerializeField] private AudioClip _nightAudioClip;

        private void OnEnable()
        {
            _dayNight.TimeDayChanged += ChangeMusic;
        }

        private void OnDisable()
        {
            _dayNight.TimeDayChanged -= ChangeMusic;
        }

        private void ChangeMusic()
        {
            _audioSource.Stop();
            _audioSource.clip = _dayNight.IsNight ? _nightAudioClip : _dayAudioClip;
            _audioSource.Play();
        }
    }
}