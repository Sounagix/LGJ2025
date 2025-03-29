using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField]
    private int _maxHandSize;

    [SerializeField]
    private GameObject _cardHUDPrefab;

    private List<BaseCardHUD> _hand = new List<BaseCardHUD>();

    [SerializeField]
    private List<BaseCardSO> test = new List<BaseCardSO>();


    public void Start()
    {
        //CreateHand();
        TestCreateHand();
    }

    private void TestCreateHand()
    {
        foreach (var c in test)
        {
            CreateHUDCard(c);
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
        GameObject currentHudCard = Instantiate(_cardHUDPrefab, transform);
        currentHudCard.GetComponent<BaseCardHUD>().Initialize(baseCardSO);
        //_hand.Add(currentHudCard);
    }
}
