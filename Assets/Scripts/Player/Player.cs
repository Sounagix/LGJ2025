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
            GameManagerActions.OnGameStateChange(GAME_STATE.SELECTION_STATE);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameManagerActions.OnSceneChange += OnSceneChange;
    }

    private void OnDisable()
    {
        GameManagerActions.OnSceneChange -= OnSceneChange;
    }

    private void OnSceneChange(SCENES sCENES)
    {
        if (sCENES.Equals(SCENES.MAIN_MENU) || sCENES.Equals(SCENES.MAIN_MENU))
            Destroy(gameObject);
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

    public void RemoveCard(string name)
    {
        BaseCardSO card = _deck.Find(c => c._cardName == name);
        if (card)
            _deck.Remove(card);
    }

}
