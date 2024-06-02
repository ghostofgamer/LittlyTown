using EndGameContent;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons.EndScreen
{
    public class CancelLastStepButton : AbstractButton
    {
        [SerializeField] private MovesKeeper _movesKeeper;
        [SerializeField] private EndPositionScreen _endPositionScreen;
        [SerializeField] private GameLevelScreen _gameLevelScreen;
        
        protected override void OnClick()
        {
            _endPositionScreen.Close();
            _gameLevelScreen.Open();
            AudioSource.PlayOneShot(AudioSource.clip);
            _movesKeeper.CancelLastStep();
        }
    }
}
