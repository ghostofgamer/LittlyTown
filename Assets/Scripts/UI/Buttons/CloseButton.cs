using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class CloseButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _abstractScreen;
        [SerializeField]private BlurScreen _blurScreen;
        
        protected override void OnClick()
        {
            Close();
        }

        public void Close()
        {
            _abstractScreen.Close();
                        _blurScreen.BlurDeactivation();
        }
    }
}
