using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [SerializeField]
    private Button _deckButton;

    [SerializeField]
    private GameObject _HudCardPrefab;

    [SerializeField]
    private float _deckToHandTime;

    [SerializeField]
    private HandManager _handManager;

    List<BaseCardSO> _deck = new();

    private void OnEnable()
    {
        GenerateDeck();
    }

    private void OnDisable()
    {
        
    }

    public void GenerateDeck()
    {
        
        foreach (BaseCardSO card in Player.Instance.GetDeck())
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

        GameObject currentCard = Instantiate(_HudCardPrefab, transform);
        BaseCardHUD hudCard = currentCard.GetComponent<BaseCardHUD>();
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
