using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotsManager : MonoBehaviour
{
    [SerializeField]
    private int _numOfSlots;

    [SerializeField]
    private GameObject _slotPrefab;

    [SerializeField]
    private GameObject _baseCardGOPrefab;

    private SpriteRenderer _spriteRenderer;

    private List<GameObject> _slots = new List<GameObject>();
    private List<bool> _slotsStatus = new List<bool>{ false, false, false, false, false };


private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        CreateSlots();
        AddMainCard();
    }

    private void CreateSlots()
    {
        Vector2 sprSize = _spriteRenderer.size;
        float spacing = sprSize.x / (_numOfSlots + 1); // espacio uniforme entre slots

        for (int i = 0; i < _numOfSlots; i++)
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

    private void AddMainCard()
    {
        int index = _slots.Count / 2;
        GameObject mainCard = Instantiate(_baseCardGOPrefab, _slots[index].transform);
        mainCard.GetComponent<BaseCardOG>().Initialize(Player.Instance.GetMainCard());
        _slotsStatus[index] = true;
    }
}
