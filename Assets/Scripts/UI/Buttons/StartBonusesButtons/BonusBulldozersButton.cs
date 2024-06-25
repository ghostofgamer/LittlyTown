namespace UI.Buttons.StartBonusesButtons
{
    public class BonusBulldozersButton : BonusesButton
    {
        protected override void SelectBonus()
        {
            ActivateChoose();
            BonusesStart.IncreaseAmountBulldozers(Reward, Price);
        }
    }
}