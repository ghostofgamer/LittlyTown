using UI.Buttons;
using UnityEngine;


public class StartButton : AbstractButton
{
    [SerializeField] private MapGenerator _mapGenerator;
    
    protected override void OnClick()
    {
        _mapGenerator.Generation();
    }
}
