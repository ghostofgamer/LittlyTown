using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class OpenButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _abstractScreen;
        [SerializeField]private BlurScreen _blurScreen;
        
        protected override void OnClick()
        {
            _blurScreen.BlurActivation();
            _abstractScreen.Open();
        }
    }
}
