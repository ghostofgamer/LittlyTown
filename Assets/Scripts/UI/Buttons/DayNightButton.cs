using DayNightContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class DayNightButton : AbstractButton
    {
        [SerializeField] private Image _dayImage;
        [SerializeField] private Image _nightImage;
        [SerializeField] private DayNight _dayNight;

        protected override void OnEnable()
        {
            base.OnEnable();
            _dayNight.TimeDayChanged += OnChangeImage;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _dayNight.TimeDayChanged -= OnChangeImage;
        }

        private void Start()
        {
            OnChangeImage();
        }

        protected override void OnClick()
        {
            AudioSource.PlayOneShot(AudioSource.clip);
            _dayNight.ChangeDayTime();
        }

        private void OnChangeImage()
        {
            if (_dayImage != null)
                _dayImage.enabled = !_dayNight.IsNight;

            _nightImage.enabled = _dayNight.IsNight;
        }
    }
}