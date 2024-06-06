namespace UI.Buttons.StartBonusesButtons
{
    public class BonusReplaceButton : BonusesButton
    {
        protected override void SelectBonus()
        {
            ActivateChoose();
            BonusesStart.IncreaseAmountReplaces(Reward, Price);
        }
    }
}
