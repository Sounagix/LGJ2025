using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Player : MonoBehaviour
{
    private BaseCardSO _mainCard;

    private List<BaseCardSO> _deck = new List<BaseCardSO>();

    public static Player Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMainCard(BaseCardSO baseCardSO)
    {
        _mainCard = baseCardSO;
    }

    public void AddCardToDeck(BaseCardSO baseCardSO)
    {
        _deck.Add(baseCardSO);
    }

    public BaseCardSO GetMainCard()
    {
        return _mainCard;
    }

    public List<BaseCardSO> GetDeck()
    {
        return _deck;
    }


}
