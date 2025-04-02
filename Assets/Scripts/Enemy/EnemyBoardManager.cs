using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardManager : MonoBehaviour
{
    private List<CombatCardSO> _enemyDeck = new List<CombatCardSO>();

    [SerializeField]
    private CardSlotsManager _slotsManager;

    private void OnEnable()
    {
        //CreateSlots();
        //CreateCard();
    }

    public void SetCards(List<BaseCardSO> cards)
    {
        _slotsManager.CreateDeckCard(cards);
    }




    /*private void CreateSlots()
    {
        Vector2 sprSize = _spriteRenderer.size;
        float spacing = sprSize.x / (_numberOfSlots + 1); // espacio uniforme entre slots

        for (int i = 0; i < _numberOfSlots; i++)
        {
            // Posición relativa al centro del sprite
            float x = -sprSize.x / 2f + spacing * (i + 1);
            //float y = sprSize.y / 2f;

            Vector2 localPos = new Vector2(x, 0);
            GameObject currentSlot = Instantiate(_slotPrefab, transform);
            currentSlot.transform.localPosition = localPos;
            _slots.Add(currentSlot);
        }
    }*/
}
