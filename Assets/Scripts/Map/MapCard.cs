using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
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

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _cardType = (CARD_TYPE)UnityEngine.Random.Range(0, (int)CARD_TYPE.NULL);
    }

    public void SetUp(BoardManager boardManager, Vector2Int index)
    {
        _boardManager = boardManager;
        _index = index;
    }

    public Vector2Int GetIndex()
    {
        return _index;
    }

    private void OnMouseDown()
    {
        _boardManager.OnCardTouched(this);
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
}
