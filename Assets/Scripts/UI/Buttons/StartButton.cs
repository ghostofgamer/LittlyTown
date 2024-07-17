using CountersContent;
using MapsContent;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class StartButton : AbstractButton
    {
        [SerializeField] private StartMap _startMap;
        [SerializeField] private MoveCounter _moveCounter;
        [SerializeField] private EndMoveScreen _endMoveScreen;
        
        protected override void OnClick()
        {
            if (_moveCounter.MoveCount <= 0)
            {
                _endMoveScreen.OnOpen();
            }
            else
            {
               _startMap.StartCreate(); 
            }
        }
    }
}