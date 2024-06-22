using MapsContent;
using UI.Buttons;
using UnityEngine;

public class ChooseMapButton : AbstractButton
{
    [SerializeField] private ChooseMap _chooseMap;
    [SerializeField] private int value;
    [SerializeField] private int _index;

    public int Index => _index;
    
    protected override void OnClick()
    {
        AudioSource.PlayOneShot(AudioSource.clip);
        _chooseMap.ChangeMap(value);
    }
}