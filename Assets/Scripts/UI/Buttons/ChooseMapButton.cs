using UI.Buttons;
using UnityEngine;

public class ChooseMapButton : AbstractButton
{
    [SerializeField] private ChooseMap _chooseMap;
    [SerializeField] private int _index;

    protected override void OnClick()
    {
        AudioSource.PlayOneShot(AudioSource.clip);
        _chooseMap.ChangeMap(_index);
    }
}