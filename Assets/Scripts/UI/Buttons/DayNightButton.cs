using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

public class DayNightButton : AbstractButton
{
    [SerializeField] private Image _dayImage;
    [SerializeField] private Image _nightImage;
    [SerializeField] private DayNight _dayNight;

    protected override void OnEnable()
    {
        base.OnEnable();
        _dayNight.TimeDayChanged += ChangeImage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _dayNight.TimeDayChanged -= ChangeImage;
    }

    private void Start()
    {
        ChangeImage();
    }

    protected override void OnClick()
    {
        _dayNight.ChangeDayTime();
        
    }

    private void ChangeImage()
    {
        if (_dayImage != null)
            _dayImage.enabled = !_dayNight.IsNight;

        _nightImage.enabled = _dayNight.IsNight;
    }
}