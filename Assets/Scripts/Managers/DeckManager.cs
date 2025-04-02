using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [SerializeField]
    private Button _deckButton;

    [SerializeField]
    private GameObject _combatCardPrefab, _healingCardPrefab;

    [SerializeField]
    private float _deckToHandTime;

    [SerializeField]
    private HandManager _handManager;

    List<BaseCardSO> _deck = new();

    [SerializeField]
    private bool _testActive;

    [SerializeField]
    private List<BaseCardSO> _testList = new();

    private void OnEnable()
    {
        GenerateDeck();
    }

    private void OnDisable()
    {
        
    }

    public void GenerateDeck()
    {
        List<BaseCardSO> deck = _testActive ? _testList : Player.Instance.GetDeck();
        foreach (BaseCardSO card in deck)
        {
            _deck.Add(Instantiate(card));
            
        }
        //_deck.CopyTo(0, Player.Instance.GetDeck(),); //= Player.Instance.GetDeck();
        if (_deck.Count > 0)
        {
            _deckButton.gameObject.SetActive(true);
            _deckButton.onClick.RemoveAllListeners();
            _deckButton.onClick.AddListener(
                delegate ()
                {
                    DrawCard();
                });
        }
        else
        {
            _deckButton.gameObject.SetActive(false);
        }
    }

    private void DrawCard()
    {
        BaseCardHUD currentCard = CreateHUDCard();
        StartCoroutine(MoveCardFromDeckToHand(currentCard, _handManager.transform.position));
        if (_deck.Count <= 0)
        {
            _deckButton.gameObject.SetActive(false);
        }
    }

    private BaseCardHUD CreateHUDCard()
    {
        int index = Random.Range(0, _deck.Count);
        GameObject currentCard = null;
        BaseCardHUD hudCard = null;
        switch (_deck[index].cARD)
        {
            case CARD_HUD_TYPE.COMBAT:
                {
                    currentCard = Instantiate(_combatCardPrefab, transform);
                    hudCard = currentCard.GetComponent<CombatCardHUD>();
                }
                break;
            case CARD_HUD_TYPE.HEALING:
                {
                    currentCard = Instantiate(_healingCardPrefab, transform);
                    hudCard = currentCard.GetComponent<HealingCardHUD>();
                }
                break;
            case CARD_HUD_TYPE.NULL:
                break;
        }
        hudCard.Initialize(_deck[index]);
        currentCard.transform.position = transform.position;
        _deck.RemoveAt(index);
        return hudCard;
    }

    private IEnumerator MoveCardFromDeckToHand(BaseCardHUD card, Vector2 target)
    {
        yield return new WaitForSecondsRealtime(_deckToHandTime);
        Vector2 startPos = card.transform.position;
        float elapsed = 0f;

        while (elapsed < _deckToHandTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _deckToHandTime);
            card.transform.position = Vector2.Lerp(startPos, target, t);
            yield return null;
        }

        card.transform.position = target;
        OnCardTouchHand(card);
    }

    private void OnCardTouchHand(BaseCardHUD card)
    {
        _handManager.AddCard(card);
    }

    public void Clean()
    {
        for (int i = 0; i < _deck.Count; i++)
        {
            Destroy(_deck[i]);
        }
        _deck.Clear();
    }

    public void RemoveCard(BaseCardHUD card)
    {
        Player.Instance.RemoveCard(card.GetName());
    }
}
