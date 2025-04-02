using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public enum CARD_TYPE
{
    ENEMY,
    REWARD,
    BLOCK,
    NULL
}

public class MapCard : MonoBehaviour
{
    private BoardManager _boardManager;

    private SpriteRenderer _spriteRenderer;

    private Vector2Int _index;

    private CARD_TYPE _cardType = CARD_TYPE.NULL;

    private REWARD_TYPE _cARDVALUE = REWARD_TYPE.IRON;

    private List<BaseCardSO> _cardsOnMapCard = new();

    [SerializeField]
    private int _numOfCards;

    [SerializeField]
    private float _rewardsChance, _enemychance;

    [SerializeField]
    private float _maxChance;

    [SerializeField]
    private List<CombatCardSO> _ironCards = new();

    [SerializeField]
    private List<CombatCardSO> _silverCards = new();

    [SerializeField]
    private List<CombatCardSO> _goldCards = new();

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetUpCard();
    }

    private void SetUpCard()
    {
        _cARDVALUE = (REWARD_TYPE)UnityEngine.Random.Range(1, (int)REWARD_TYPE.SIZE);
        float chance = UnityEngine.Random.Range(0, _maxChance);
        if (chance <= _rewardsChance)
        {
            _cardType = CARD_TYPE.REWARD;
        }
        else if (chance <= _enemychance)
        {
            _cardType = CARD_TYPE.ENEMY;
            _numOfCards = UnityEngine.Random.Range(0, _numOfCards);
            for (int i = 0; i < _numOfCards; i++)
            {

                _cardsOnMapCard.Add(GetCard(_cARDVALUE));
            }
        }
        else
        {
            _cardType = CARD_TYPE.BLOCK;
        }
    }

    public void SetUp(BoardManager boardManager, Vector2Int index)
    {
        _boardManager = boardManager;
        _index = index;
    }

    private BaseCardSO GetCard(REWARD_TYPE rEWARD_TYPE)
    {
        BaseCardSO currentCard = null;
        int index = 0;
        switch (rEWARD_TYPE)
        {
            case REWARD_TYPE.IRON:
                index = UnityEngine.Random.Range(1, _ironCards.Count);
                currentCard = _ironCards[index];
                break;
            case REWARD_TYPE.SILVER:
                index = UnityEngine.Random.Range(0, _silverCards.Count);
                currentCard = _silverCards[index];
                break;
            case REWARD_TYPE.GOLD:
                index = UnityEngine.Random.Range(0, _goldCards.Count);
                currentCard = _goldCards[index];
                break;
            case REWARD_TYPE.SIZE:
                break;
        }
        return currentCard;
    }

    public Vector2Int GetIndex()
    {
        return _index;
    }

    private void OnMouseDown()
    {
        if(GameManager.Instance.GetGAME_STATE().Equals(GAME_STATE.MAP_STATE))
            _boardManager.OnCardTouched(this);
    }

    public List<BaseCardSO> GetCards()
    {
        return _cardsOnMapCard;
    }

    public CARD_TYPE GetCardType()
    {
        return _cardType;
    }

    public void OnCardSelected()
    {
        _spriteRenderer.color = Color.gray;
    }

    public void OnCardUnSelected()
    {
        _spriteRenderer.color = Color.white;
    }

    public void ChangeToBlock()
    {
        _cardType = CARD_TYPE.BLOCK;
    }
}
