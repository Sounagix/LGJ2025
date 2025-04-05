public class DefenseBuffCardHUD : SupportCardHUD
{
    private int _valueOfDefenseBuff;

    public override void Initialize(BaseCardSO card, CardSlotsManager cardSlotsManager = null)
    {
        base.Initialize(card, cardSlotsManager);
        cARD = CARD_HUD_TYPE.DEFENSE_BUFF;

        DefenseBuffCardSO defenseBuffCard = card as DefenseBuffCardSO;

        if (defenseBuffCard)
        {
            _valueOfDefenseBuff = defenseBuffCard._defenseBuffValue;
        }
    }

    public int GetDefenseValue()
    {
        return _valueOfDefenseBuff;
    }
}
