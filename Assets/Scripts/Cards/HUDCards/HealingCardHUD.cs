public class HealingCardHUD : SupportCardHUD
{
    private int _valueOfHealing;

    public override void Initialize(BaseCardSO card, CardSlotsManager cardSlotsManager = null)
    {
        base.Initialize(card, cardSlotsManager);
        cARD = CARD_HUD_TYPE.HEALING;

        HealingCardSO healingCard = card as HealingCardSO;

        if (healingCard)
        {
            _valueOfHealing = healingCard._valueOfHealing;
        }
    }

    public int GetHealingValue()
    {
        return _valueOfHealing;
    }
}
