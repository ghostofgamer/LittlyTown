using System.Collections;
using System.Collections.Generic;
using UI.Buttons;
using UnityEngine;

public abstract class SettingsChangeButton : AbstractButton
{
    [SerializeField] private Settings _settings;
    
    protected Settings Settings => _settings;
}
