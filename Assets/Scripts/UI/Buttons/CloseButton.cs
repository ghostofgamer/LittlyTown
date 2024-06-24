using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class CloseButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _abstractScreen;

        protected override void OnClick()
        {
            Close();
        }

        private void Close()
        {
            _abstractScreen.Close();
        }
    }
}