using System.Collections;
using System.Collections.Generic;
using Enums;
using SaveAndLoad;
using UI;
using UI.Buttons;
using UnityEngine;

public abstract class SettingsChangeButton : AbstractButton
{
    [SerializeField] private Settings _settings;
    [SerializeField] private SettingsModes _settingsModes;
    [SerializeField] private Save _save;
    [SerializeField] private Load _load;
    [SerializeField] private int _firstValue;
    [SerializeField] private int _secondValue;
    [SerializeField] private int _startValue;
    [SerializeField] private bool _isToggleOn;

    private int _currentValue;

    protected SettingsModes SettingsModes => _settingsModes;
    
    protected Settings Settings => _settings;

    protected bool IsToggleOn => _isToggleOn;
    
    protected int FirstValue => _firstValue;
    
    protected int SecondValue => _secondValue;

    private void Start()
    {
        LoadValue();
    }

    protected abstract void ChangeValue();

    protected override void OnClick()
    {
        _isToggleOn = !_isToggleOn;
        AudioSource.PlayOneShot(AudioSource.clip);
        ChangeValue();
    }

    protected void SaveValue()
    {
        _save.SetData(_settingsModes.ToString(), _isToggleOn ? _firstValue : _secondValue);
    }

    private void LoadValue()
    {
        _currentValue = _load.Get(_settingsModes.ToString(), _startValue);
        _isToggleOn = _currentValue == _firstValue;
        ChangeValue();
    }
}
