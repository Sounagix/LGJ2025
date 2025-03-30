using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _slotPrefab;

    [SerializeField]
    private GameObject _cardOGPrefab;

    [SerializeField]
    private int _numberOfSlots;

    [SerializeField]
    private List<GameObject> _slots = new List<GameObject>();

    [SerializeField]
    private List<BaseCardSO> _enemyDeck = new List<BaseCardSO>();

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        CreateSlots();
        CreateCard();
    }

    private void CreateCard()
    {
        int cont = 0;
        foreach (var card in _enemyDeck)
        {
            CreateCards(card, _slots[cont]);
            cont++;
        }
    }

    private void CreateCards(BaseCardSO card, GameObject slot)
    {
        GameObject currentCard = Instantiate(_cardOGPrefab, slot.transform);
        currentCard.transform.position = slot.transform.position + -Vector3.forward * 1.2f;
        currentCard.GetComponent<BaseCardOG>().Initialize(card);
        currentCard.tag = "EnemyCard";
    }

    private void CreateSlots()
    {
        Vector2 sprSize = _spriteRenderer.size;
        float spacing = sprSize.x / (_numberOfSlots + 1); // espacio uniforme entre slots

        for (int i = 0; i < _numberOfSlots; i++)
        {
            // Posición relativa al centro del sprite
            float x = -sprSize.x / 2f + spacing * (i + 1);
            //float y = sprSize.y / 2f;

            Vector2 localPos = new Vector2(x, 0);
            GameObject currentSlot = Instantiate(_slotPrefab, transform);
            currentSlot.transform.localPosition = localPos;
            _slots.Add(currentSlot);
        }
    }
}
