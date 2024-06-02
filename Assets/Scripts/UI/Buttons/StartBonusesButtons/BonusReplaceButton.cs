namespace UI.Buttons.StartBonusesButtons
{
    public class BonusReplaceButton : BonusesButton
    {
        protected override void SelectBonus()
        {
            BonusesStart.IncreaseAmountReplaces(Reward, Price);
        }
    }
}
