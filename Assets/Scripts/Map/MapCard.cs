using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    private SpriteRenderer _cardImage;

    [SerializeField]
    private List<CombatCardSO> _ironCards = new();

    [SerializeField]
    private List<CombatCardSO> _silverCards = new();

    [SerializeField]
    private List<CombatCardSO> _goldCards = new();

    [SerializeField]
    private List<CombatCardSO> _bossCards = new();

    [SerializeField]
    private int _numOfCardForFirstLap, _numOfCardForSecondLap, _numOfCardForThirdLap, _numOfCardsToFinishGame;

    private bool _touched = false;

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
        float chance = UnityEngine.Random.Range(0, _maxChance);
        if (chance <= _rewardsChance)
        {
            _cardType = CARD_TYPE.REWARD;
        }
        else if (chance <= _enemychance)
        {
            _cardType = CARD_TYPE.ENEMY;
        }
        else
        {
            _cardType = CARD_TYPE.BLOCK;
        }
    }

    public void SetUpCards(int numOfMapCardsTouched)
    {
        _numOfCards = UnityEngine.Random.Range(1, _numOfCards);
        if (numOfMapCardsTouched >= _numOfCardsToFinishGame)
        {
            // 5 acompañando al boss
            _numOfCards = 4;
            _cARDVALUE = REWARD_TYPE.GOLD;
            for (int i = 0; i < _numOfCards; i++)
            {
                _cardsOnMapCard.Add(GetCard(_cARDVALUE));
            }
            _cARDVALUE = REWARD_TYPE.BOSS;
            _cardsOnMapCard.Add(GetCard(_cARDVALUE));
        }
        else
        {
            if (numOfMapCardsTouched <= _numOfCardForFirstLap)
            {
                _cARDVALUE = REWARD_TYPE.IRON;
            }
            else if (numOfMapCardsTouched <= _numOfCardForSecondLap)
            {
                _cARDVALUE = REWARD_TYPE.SILVER;
            }
            else if (numOfMapCardsTouched <= _numOfCardForThirdLap)
            {
                _cARDVALUE = REWARD_TYPE.GOLD;
            }

            for (int i = 0; i < _numOfCards; i++)
            {
                _cardsOnMapCard.Add(GetCard(_cARDVALUE));
            }
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
                index = UnityEngine.Random.Range(0, _ironCards.Count);
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
            case REWARD_TYPE.BOSS:
                index = UnityEngine.Random.Range(0, _bossCards.Count);
                currentCard = _bossCards[index];
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

    public REWARD_TYPE GetCardReward()
    {
        return _cARDVALUE;
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
        _cardImage.color = new Color(100, 85, 85, 223);
        _cardType = CARD_TYPE.BLOCK;
        _touched = true;
    }

    public bool IsUsed()
    {
        return _touched;
    }
}
