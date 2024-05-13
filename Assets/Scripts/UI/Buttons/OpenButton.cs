using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class OpenButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _abstractScreen;

        protected override void OnClick()
        {
            _abstractScreen.Open();
        }
    }
}
