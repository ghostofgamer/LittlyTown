namespace UI.Buttons.StartBonusesButtons
{
    public class BonusGoldButton : BonusesButton
    {
        protected override void SelectBonus()
        {
            ActivateChoose();
            BonusesStart.IncreaseAmountGolds(Reward, Price);
        }
    }
}