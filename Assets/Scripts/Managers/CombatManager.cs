using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CombatActions
{
    public static Action<BaseCardHUD> OnDropDragedCard;

    public static Action<BaseCardOG> OnCardOGSelected;
} 


public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private Button _backButton;

    [SerializeField]
    private HandManager _handManager;

    [SerializeField]
    private CardSlotsManager _cardSlotsManager;

    [SerializeField]
    private float _timeToAttack;

    private BaseCardOG _currentCardSelected;

    private BaseCardOG _currentTargetSelected;

    private void OnEnable()
    {
        CombatActions.OnDropDragedCard += OnDropDragedCard;
        CombatActions.OnCardOGSelected += OnCardOGSelected;
    }

    private void OnDisable()
    {
        CombatActions.OnDropDragedCard -= OnDropDragedCard;
        CombatActions.OnCardOGSelected -= OnCardOGSelected;
    }

    private void Awake()
    {
        _backButton.onClick.AddListener(() => GameManager.Instance.LoadScene(SCENES.GAME));
    }

    private void OnDropDragedCard(BaseCardHUD droppedHUD)
    {
        Vector3[] corners = new Vector3[4];
        RectTransform droppedHUDRectTransform = droppedHUD.GetComponent<RectTransform>();

        droppedHUDRectTransform.GetWorldCorners(corners);

        bool foundSlot = false;
        int cont = 0;

        do
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(corners[cont]) ;
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Slot") 
                && _cardSlotsManager.IsSlotFree(hit.collider.gameObject))
            {
                ConvertHUDCardToGameCard(droppedHUD, hit.collider.gameObject);
                foundSlot = true;
            }

            cont++;
        }
        while (!foundSlot && cont < 4);

        if (!foundSlot)
            droppedHUD.NotSelectedOnSlot();
    }

    private void ConvertHUDCardToGameCard(BaseCardHUD hudCard, GameObject slot)
    {
        _cardSlotsManager.AddGameCardFromHudCard(hudCard, slot);
        _handManager.RemoveCardFromHand(hudCard);
    }

    private void OnCardOGSelected(BaseCardOG oG)
    {
        if (!IsAllyCard(oG))
        {
            if (_currentCardSelected && !_currentTargetSelected)
            {
                _currentTargetSelected = oG;
                AttackEnemyCard();
            }
            else if (_currentCardSelected && _currentTargetSelected)
            {
                _currentTargetSelected.UnSelecCard();
                _currentTargetSelected = oG;
                AttackEnemyCard();
            }
        }
        else if (_currentCardSelected)
        {
            _currentCardSelected.UnSelecCard();
            _currentCardSelected = oG;
        }
        else
        {
            _currentCardSelected = oG;
        }
    }

    private void AttackEnemyCard()
    {
        StartCoroutine(MoveCard(_currentCardSelected, _currentTargetSelected.transform.position, _timeToAttack));
    }

    private void OnArriveToEnemyPosition()
    {
        _currentCardSelected.UnSelecCard();
        _currentTargetSelected.UnSelecCard();
        _currentCardSelected = null;
        _currentTargetSelected = null;
    }

    private bool IsAllyCard(BaseCardOG card)
    {
        return !card.CompareTag("EnemyCard");
    }

    private IEnumerator MoveCard(BaseCardOG baseCardOG, Vector2 target, float duration)
    {
        Vector2 startPos = baseCardOG.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            baseCardOG.transform.position = Vector2.Lerp(startPos, target, t);
            yield return null;
        }

        baseCardOG.transform.position = target; // asegúrate de terminar exactamente en el destino

        startPos = baseCardOG.transform.position;
        target = baseCardOG.transform.parent.transform.position;
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            baseCardOG.transform.position = Vector2.Lerp(startPos, target, t);
            yield return null;
        }
        baseCardOG.transform.position = target;
        OnArriveToEnemyPosition();
    }
}
