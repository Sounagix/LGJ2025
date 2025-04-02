using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (blocked) return;
        if (!_cardOnGame)
            CombatActions.OnHealingCardDroped?.Invoke(this);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (blocked || _cardOnGame) return;
        switch (GameManager.Instance.GetGAME_STATE())
        {
            case GAME_STATE.COMBAT_STATE:
                _selected = true;
                _cardImg.color = Color.gray;
                CombatActions.OnCardOGSelected?.Invoke(this);
                break;
            case GAME_STATE.REWARD_STATE:
                Player.Instance.AddCardToDeck(_baseCardSO);
                RewardManagerActions.OnRewardCollected?.Invoke();
                break;
            case GAME_STATE.NULL:
                break;
        }
    }
}
