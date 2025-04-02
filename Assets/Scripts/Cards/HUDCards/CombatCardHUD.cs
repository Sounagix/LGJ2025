using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatCardHUD : BaseCardHUD
{
    private int _attackDamage;

    private int _lifePoints;

    private int _defensePoints;

    [SerializeField]
    protected TextMeshProUGUI _hpText, _blockText, _attackText;

    [SerializeField]
    private ParticleSystem _healingPTC;

    public override void Initialize(BaseCardSO card, CardSlotsManager cardSlotsManager = null)
    {
        base.Initialize(card, cardSlotsManager);
        cARD = CARD_HUD_TYPE.COMBAT;
        CombatCardSO combatCardSO = card as CombatCardSO;
        if (combatCardSO)
        {
            _attackDamage = combatCardSO._damage;
            _lifePoints = combatCardSO._life;
            _defensePoints = combatCardSO._block;
            _hpText.text = _lifePoints.ToString();
            _blockText.text = _defensePoints.ToString();
            _attackText.text = _attackDamage.ToString();
        }
    }

    public int GetDamage()
    {
        return _attackDamage;
    }

    public int GetLifePoints()
    {
        return _lifePoints;
    }

    public int GetDefensePoints()
    {
        return _defensePoints;
    }

    public bool SubtractLife(int value)
    {
        _lifePoints -= value;
        _hpText.text = _lifePoints.ToString();
        return _lifePoints <= 0;
    }

    public void HealCard(int v)
    {
        _healingPTC.Play();
        _lifePoints += v;
        _hpText.text = _lifePoints.ToString();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (blocked) return;
        if (GameManager.Instance.GetGAME_STATE().Equals(GAME_STATE.COMBAT_STATE))
            CombatActions.OnCombatCardDroped?.Invoke(this);
    }
}
