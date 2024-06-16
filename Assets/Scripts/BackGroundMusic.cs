using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private DayNight _dayNight;
    [SerializeField]private AudioClip _audioClip;

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
        _audioSource.PlayOneShot(_dayNight.IsNight ? _audioClip : _audioSource.clip);
    }
}
