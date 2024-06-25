using UI.Buttons.StartBonusesButtons;
using UnityEngine;
using UpgradesContent;

namespace UI.Screens
{
    public class StartGameScreen : AbstractScreen
    {
        [SerializeField] private BonusesStart _bonusesStart;
        [SerializeField] private BonusesButton[] _bonusesButton;

        public override void OnOpen()
        {
            base.OnOpen();
            _bonusesStart.ResetValue();

            foreach (var bonusesButton in _bonusesButton)
                bonusesButton.DeactivateChoose();
        }
    }
}