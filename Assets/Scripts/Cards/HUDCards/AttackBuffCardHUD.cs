using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackBuffCardHUD : SupportCardHUD
{
    private int _valueOfAttackBuff;

    public override void Initialize(BaseCardSO card, CardSlotsManager cardSlotsManager = null)
    {
        base.Initialize(card, cardSlotsManager);
        cARD = CARD_HUD_TYPE.ATTACK_BUFF;

        AttackBuffCardSO attackBuffCard = card as AttackBuffCardSO;

        if (attackBuffCard)
        {
            _valueOfAttackBuff = attackBuffCard._attackIncreased;
        }
    }

    public int GetAttackBuffValue()
    {
        return _valueOfAttackBuff;
    }
}
