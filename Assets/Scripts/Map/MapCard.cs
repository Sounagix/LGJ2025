using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCard : MonoBehaviour
{
    private BoardManager _boardManager;

    private SpriteRenderer _spriteRenderer;

    private Vector2Int _index;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void OnCardSelected()
    {
        _spriteRenderer.color = Color.gray;
    }

    public void OnCardUnSelected()
    {
        _spriteRenderer.color = Color.white;
    }
}
