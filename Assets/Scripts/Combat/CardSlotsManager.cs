using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotsManager : MonoBehaviour
{
    [SerializeField]
    private int _numOfSlots;

    [SerializeField]
    private GameObject _hudCardPrefab;

    [SerializeField]
    private string _cardTag;

    private List<GameObject> _slots = new List<GameObject>();

    private List<bool> _slotsStatus = new List<bool>{ true, true, true, true, true };

    private List<RectTransform> _slotRectTransforms = new List<RectTransform>();

    private List<BaseCardHUD> _cards = new List<BaseCardHUD>();

    private void Start()
    {
        CreateSlots();
        for (int i = 0; i < _numOfSlots; i++)
        {
            _cards.Add(null);
        }
    }

    private void CreateSlots()
    {
        GameObject slot = transform.GetChild(0).gameObject;
        _slots.Add(slot);
        _slotRectTransforms.Add(slot.GetComponent<RectTransform>());
        for (int i = 0; i < _numOfSlots - 1; i++)
        {
            GameObject nextSlot = Instantiate(slot, transform);
            _slots.Add(nextSlot);
            _slotRectTransforms.Add(nextSlot.GetComponent<RectTransform>());
        }
    }

    public void BlockSelection()
    {
        foreach (BaseCardHUD card in _cards)
        {
            if (card)
                card.BlockCard();
        }
    }

    public void UnblockSelection()
    {
        foreach (BaseCardHUD card in _cards)
        {
            if (card)
                card.UnlockCard();
        }
    }


    public bool IsSlotFree(int index)
    {
        return _slotsStatus[index];
    }

    public List<RectTransform> GetSlots()
    {
        return _slotRectTransforms;
    }

    public void SetSlotAsUsed(BaseCardHUD card, int index)
    {
        _slotsStatus[index] = false;
        _cards[index] = card;
    }

    public void CreateMainCard(BaseCardSO baseCardSO)
    {
        int index = (int)_slots.Count / 2;
        CreateHudCard(baseCardSO, index);
    }

    public void CreateDeckCard(List<BaseCardSO> cards)
    {
        int index = 0;
        foreach (BaseCardSO baseCardSO in cards)
        {
            if (!_slotsStatus[index])
            {
                index++;
                continue;
            }
            CreateHudCard(baseCardSO, index);
        }
    }

    private void CreateHudCard(BaseCardSO card, int index)
    {
        GameObject hudCard = Instantiate(_hudCardPrefab, _slots[index].transform);
        BaseCardHUD hud = hudCard.GetComponent<BaseCardHUD>();
        hud.Initialize(card, this);

        hudCard.transform.SetParent(_slots[index].transform);
        hudCard.transform.localPosition = Vector3.zero;
        hudCard.tag = _cardTag;
        _slotsStatus[index] = false;
        _cards[index] = hud;
    }

    public BaseCardHUD GetRandomCard()
    {
        bool validCard = false;
        int index = 0;
        List<BaseCardHUD> cardsAvarible = _cards.FindAll(c => c != null);
        if (cardsAvarible.Count == 0)
            throw new Exception("No cards! on the deck");

        do
        {
            index = UnityEngine.Random.Range(0, cardsAvarible.Count);
            validCard = cardsAvarible[index];
        }
        while (!validCard);

        return cardsAvarible[index];
    }

    public void KillCard(BaseCardHUD defender)
    {
        if (_cards.Contains(defender))
        {
            defender.BlockCard();
            defender.KillCard();
        }
    }

    public void DestroyCard(BaseCardHUD card)
    {
        int index = _cards.IndexOf(card);
        _slotsStatus[index] = true;
        _cards[index] = null;
        Destroy(card.gameObject);
    }

    public bool HaveCards()
    {
        return _cards.FindAll(c => c != null).Count > 0;
    }

    public void Clean()
    {

        for (int i = 0; i < _numOfSlots; i++)
        {
            _slotsStatus[i] = true;
            if (_cards[i] != null)
            {
                Destroy(_cards[i].gameObject);
                _cards[i] = null;
            }
        }
    }

    /*private void CreateSlots()
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
        _slotsStatus[index] = false;
    }

    public List<GameObject> GetSlots()
    {
        return _slots;
    }



    public void AddGameCardFromHudCard(BaseCardHUD hudCard, GameObject slot)
    {
        int index = _slots.IndexOf(slot);
        GameObject newCard = Instantiate(_baseCardGOPrefab, slot.transform);
        newCard.GetComponent<BaseCardOG>().Initialize(hudCard);
        _slotsStatus[index] = true;
    }*/
}
