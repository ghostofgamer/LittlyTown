namespace UI.Buttons.StartBonusesButtons
{
    public class BonusGoldButton : BonusesButton
    {
        protected override void SelectBonus()
        {
            BonusesStart.IncreaseAmountGolds(Reward, Price);
        }
    }
}