using MapsContent;
using UnityEngine;

namespace UI.Buttons
{
    public class StartButton : AbstractButton
    {
        [SerializeField]private StartMap _startMap;

        protected override void OnClick()
        {
            _startMap.StartCreate();
        }
    }
}