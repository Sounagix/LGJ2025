using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    [SerializeField]
    private int _maxHandSize;

    [SerializeField]
    private GameObject _cardHUDPrefab;

    private List<BaseCardHUD> _hand = new List<BaseCardHUD>();

    [SerializeField]
    private List<BaseCardSO> test = new List<BaseCardSO>();

    private void OnEnable()
    {
        CreateHand();
        //TestCreateHand();
    }

    private void TestCreateHand()
    {
        Vector2 boardSize = GetComponent<RectTransform>().sizeDelta;
        float spacing = boardSize.x / (_maxHandSize + 1);
        float startX = -boardSize.x / 2 + spacing;

        for (int i = 0; i < test.Count; i++)
        {
            GameObject currentHudCard = Instantiate(_cardHUDPrefab, transform);
            RectTransform cardRT = currentHudCard.GetComponent<RectTransform>();

            BaseCardHUD baseCardHUD = currentHudCard.GetComponent<BaseCardHUD>();
            baseCardHUD.Initialize(test[i]);

            float x = startX + spacing * i;
            cardRT.anchoredPosition = new Vector2(x, 0); // Puedes ajustar el eje Y si quieres

            _hand.Add(baseCardHUD);
        }

    }

    private void CreateHand()
    {
        foreach (var c in Player.Instance.GetDeck())
        {
            CreateHUDCard(c);
        }
    }

    private void CreateHUDCard(BaseCardSO baseCardSO)
    {
        ////List<BaseCardSO> 
        //Vector2 boardSize = GetComponent<RectTransform>().sizeDelta;
        //float spacing = boardSize.x / (_maxHandSize + 1);
        //float startX = -boardSize.x / 2 + spacing;
        //
        //for (int i = 0; i < test.Count; i++)
        //{
        //    GameObject currentHudCard = Instantiate(_cardHUDPrefab, transform);
        //    RectTransform cardRT = currentHudCard.GetComponent<RectTransform>();
        //
        //    BaseCardHUD baseCardHUD = currentHudCard.GetComponent<BaseCardHUD>();
        //    baseCardHUD.Initialize(test[i]);
        //
        //    float x = startX + spacing * i;
        //    cardRT.anchoredPosition = new Vector2(x, 0); // Puedes ajustar el eje Y si quieres
        //
        //    _hand.Add(baseCardHUD);
        //}
    }

    public List<BaseCardHUD> GetHand()
    {
        return _hand;
    }

    public void RemoveCardFromHand(BaseCardHUD hudCard)
    {
        _hand.Remove(hudCard);
        Destroy(hudCard.gameObject);
    }

    public void Clean()
    {
        for (int i = 0; i < _hand.Count; i++)
        {
            if (_hand[i] != null)
            {
                Destroy(_hand[i].gameObject);
                _hand[i] = null;
            }
        }
    }

    public void AddCard(BaseCardHUD card)
    {
        GetComponent<HorizontalLayoutGroup>().enabled = false;
        Vector2 boardSize = GetComponent<RectTransform>().sizeDelta;
        float spacing = boardSize.x / (_maxHandSize + 1);
        float startX = -boardSize.x / 2 + spacing;

        RectTransform cardRT = card.GetComponent<RectTransform>();
        float x = startX + spacing * _hand.Count;
        cardRT.anchoredPosition = new Vector2(x, 0);
        card.transform.SetParent(transform);
        _hand.Add(card);
        GetComponent<HorizontalLayoutGroup>().enabled = true;
    }
}
