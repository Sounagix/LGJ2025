using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Player : MonoBehaviour
{
    private CombatCardSO _mainCard;

    private List<BaseCardSO> _deck = new List<BaseCardSO>();

    public static Player Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameManagerActions.OnGameStateChange?.Invoke(GAME_STATE.SELECTION_STATE);
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
        _mainCard = Instantiate(baseCardSO as CombatCardSO);
    }

    public void AddCardToDeck(BaseCardSO baseCardSO)
    {
        _deck.Add(Instantiate(baseCardSO));
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


    public void ImproveDeckCard(BaseCardSO card, int hpValue, int atkValue, int defValue)
    {
        if (_deck.Contains(card))
        {
            int index = _deck.IndexOf(card);
            CombatCardSO cmb = _deck[index].GetComponent<CombatCardSO>();
            cmb._block += defValue;
            cmb._life += hpValue;
            cmb._damage += atkValue;
        }
        else if (_mainCard.Equals(card))
        {
            _mainCard._block += defValue;
            _mainCard._life += hpValue;
            _mainCard._damage += atkValue;
        }
    }
}
