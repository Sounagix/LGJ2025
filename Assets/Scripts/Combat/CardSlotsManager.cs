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

    private CombatCardHUD _mainCard;

    private REWARD_TYPE cARD = REWARD_TYPE.SIZE;

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
            if (card == null) continue;

            card.BlockCard();
        }
    }

    public void UnblockSelection()
    {
        foreach (BaseCardHUD card in _cards)
        {
            if (card == null) continue;

            card.UnlockCard();
        }
    }


    public bool IsSlotFree(int index)
    {
        return _slotsStatus[index];
    }

    public bool IsSlotFree(GameObject slot)
    {
        if (_slots.Contains(slot))
        {
            int index = _slots.IndexOf(slot);
            return _slotsStatus[index];
        }
        else
        {
            return false;
        }

    }

    public List<RectTransform> GetSlots()
    {
        return _slotRectTransforms;
    }

    public List<RectTransform> GetCardsOnGame()
    {
        List<RectTransform> trs = new();
        foreach (BaseCardHUD card in _cards)
        {
            if (card)
            {
                trs.Add(card.GetComponent<RectTransform>());
            }
        }
        return trs;
    }

    public void SetSlotAsUsed(BaseCardHUD card, int index)
    {
        _slotsStatus[index] = false;
        _cards[index] = card;
    }

    public void SetSlotAsUsed(BaseCardHUD card, GameObject slot)
    {
        if (_slots.Contains(slot))
        {
            int index = _slots.IndexOf(slot);
            _slotsStatus[index] = false;
            _cards[index] = card;
        }

    }

    public void CreateMainCard(BaseCardSO baseCardSO)
    {
        int index = (int)_slots.Count / 2;
        _mainCard = CreateHudCard(baseCardSO, index) as CombatCardHUD;
    }

    public void CreateDeckCard(List<BaseCardSO> cards, REWARD_TYPE cARD_REWARD = REWARD_TYPE.SIZE)
    {
        cARD = cARD_REWARD;
        int index = 0;
        foreach (BaseCardSO baseCardSO in cards)
        {
            if (!_slotsStatus[index])
            {
                index++;
                continue;
            }
            CreateHudCard(baseCardSO, index);
            index++;
        }
    }

    private BaseCardHUD CreateHudCard(BaseCardSO card, int index)
    {
        GameObject hudCard = Instantiate(_hudCardPrefab, _slots[index].transform);
        BaseCardHUD hud = hudCard.GetComponent<BaseCardHUD>();
        hud.Initialize(card, this);
        hud.OnAddedToBoard();

        hudCard.transform.SetParent(_slots[index].transform);
        hudCard.transform.localPosition = Vector3.zero;
        hudCard.tag = _cardTag;
        _slotsStatus[index] = false;
        _cards[index] = hud;
        return hud;
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

    public void KillCard(CombatCardHUD defender)
    {
        if (_cards.Contains(defender))
        {
            int index = _cards.IndexOf(defender);
            _slotsStatus[index] = true;
            _cards[index] = null;
            defender.gameObject.SetActive(false);
            StartCoroutine(WaitForTheEndToDestroyCard(defender));
        }
    }

    private IEnumerator WaitForTheEndToDestroyCard(CombatCardHUD card)
    {
        yield return new WaitForEndOfFrame();
        if (card != null)
        {
            Destroy(card.gameObject);
        }
    }

    public bool HaveCards()
    {
        return _cards.FindAll(c => c != null).Count > 0;
    }

    public bool MainCardAlive()
    {
        return _mainCard && _mainCard.GetLifePoints() > 0;
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

    public REWARD_TYPE GetCardType()
    {
        return cARD;
    }
}
