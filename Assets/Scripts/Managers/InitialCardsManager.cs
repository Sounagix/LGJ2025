using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCardsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _initialCardPrefab;

    [SerializeField]
    private List<BaseCardSO> _initialCards;

    private void Start()
    {
        foreach (BaseCardSO card in _initialCards)
        {
            GameObject cardObject = Instantiate(_initialCardPrefab, transform);
            CombatCardHUD hud = cardObject.GetComponent<CombatCardHUD>();
            hud.Initialize(card);
        }
    }
}
