using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatCardHUD : BaseCardHUD
{
    private int _attackDamage;

    private int _lifePoints;

    private int _defensePoints;

    [SerializeField]
    protected TextMeshProUGUI _hpText, _blockText, _attackText;

    private UnityEngine.UI.Image _mainImage;

    public override void Initialize(BaseCardSO card, CardSlotsManager cardSlotsManager = null)
    {
        base.Initialize(card, cardSlotsManager);
        _mainImage = GetComponent<UnityEngine.UI.Image>();
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

    public override void OnPointerClick(PointerEventData eventData)
    {
        switch (GameManager.Instance.GetGAME_STATE())
        {
            case GAME_STATE.SELECTION_STATE:
                Player.Instance.AddMainCard(_baseCardSO);
                GameManager.Instance.LoadScene(SCENES.GAME);
                break;
            case GAME_STATE.COMBAT_STATE:
                if (!blocked && _cardOnGame)
                {
                    _selected = true;
                    _cardImg.color = Color.gray;
                    CombatActions.OnCardOGSelected?.Invoke(this);
                }
                break;
            case GAME_STATE.REWARD_STATE:
                Player.Instance.AddCardToDeck(_baseCardSO);
                RewardManagerActions.OnRewardCollected?.Invoke();
                break;
        }
    }


    private void BackToNormalGlow()
    {
        _mainImage.material.SetFloat("_glowIntensity", 0.0f);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (blocked || _cardOnGame) return;
        GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (blocked || _cardOnGame) return;
        if (GameManager.Instance.GetGAME_STATE().Equals(GAME_STATE.COMBAT_STATE))
            transform.position = Input.mousePosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (blocked) return;
        if (GameManager.Instance.GetGAME_STATE().Equals(GAME_STATE.COMBAT_STATE))
            CombatActions.OnCombatCardDroped?.Invoke(this);
    }

    


    public void HealCard(int v)
    {
        //_mainImage.material.SetFloat("_glowIntensity", 0.5f);
        _lifePoints += v;
        _hpText.text = _lifePoints.ToString();
        //Invoke(nameof(BackToNormalGlow), 1.5f);
    }

    public void BuffAttack(int value)
    {
        //_mainImage.material.SetFloat("_glowIntensity", 0.5f);
        _attackDamage += value;
        _attackText.text = _attackDamage.ToString();
        //Invoke(nameof(BackToNormalGlow), 1.5f);
    }

    public void BuffDefense(int value)
    {
        //_mainImage.material.SetFloat("_glowIntensity", 0.5f);
        _defensePoints += value;
        _blockText.text = _defensePoints.ToString();
        //Invoke(nameof(BackToNormalGlow), 1.5f);
    }

    public bool IsAlive()
    {
        return _lifePoints > 0;
    }
}
