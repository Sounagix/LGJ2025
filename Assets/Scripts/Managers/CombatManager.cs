using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public static class CombatActions
{
    public static Action<BaseCardHUD> OnDropDragedCard;

    public static Action<BaseCardHUD> OnCardOGSelected;
} 


public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private HandManager _handManager;

    [SerializeField]
    private CardSlotsManager _playerCardSlotManager;

    [SerializeField]
    private CardSlotsManager _enemyCardSlotManager;

    [SerializeField]
    private List<BaseCardSO> _enemiesCards = new();

    [SerializeField]
    private float _timeToAttack;

    [SerializeField]
    private float _timeToStart;

    [SerializeField]
    private BaseCardSO _mainCardTest;

    private Coroutine _startCoroutine = null;

    private BaseCardHUD _currentCardSelected;

    private BaseCardHUD _currentTargetSelected;

    private void OnEnable()
    {
        CombatActions.OnDropDragedCard += OnDropDragedCard;
        CombatActions.OnCardOGSelected += OnCardOGSelected;
        GameManagerActions.OnGameStateChange?.Invoke(GAME_STATE.COMBAT_STATE);
        _startCoroutine = StartCoroutine(WaitForStart());
    }

    private void OnDisable()
    {
        CombatActions.OnDropDragedCard -= OnDropDragedCard;
        CombatActions.OnCardOGSelected -= OnCardOGSelected;
        GameManagerActions.OnGameStateChange?.Invoke(GAME_STATE.MAP_STATE);
    }

    private IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(_timeToStart);
        AddPlayerCard();
        CreateEnemies();
        _startCoroutine = null;
    }

    private void AddPlayerCard()
    {
        BaseCardSO main = Player.Instance.GetMainCard();
        if (main == null)
            main = _mainCardTest;
        _playerCardSlotManager.CreateMainCard(main);
    }
    private void CreateEnemies()
    {
        _enemyCardSlotManager.CreateDeckCard(_enemiesCards);
    }

    private void OnDropDragedCard(BaseCardHUD droppedHUD)
    {
        RectTransform cardRT = droppedHUD.GetComponent<RectTransform>();
        List<RectTransform> slotsRT = _playerCardSlotManager.GetSlots();
        int cont = 0;
        bool finded = false;
        do
        {
            Rect rectA = GetWorldRect(cardRT);
            Rect rectB = GetWorldRect(slotsRT[cont]);

            finded = rectA.Overlaps(rectB);
            cont = !finded ? cont + 1 : cont;
        }
        while (!finded && cont < slotsRT.Count);

        if (finded && _playerCardSlotManager.IsSlotFree(cont))
        {
            _playerCardSlotManager.SetSlotAsUsed(cont);
            droppedHUD.SetCardOnSlot(slotsRT[cont].transform);
        }
        else
        {
            droppedHUD.NotSelectedOnSlot();
        }
    }

    public static Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        Vector2 size = topRight - bottomLeft;

        return new Rect(bottomLeft, size);
    }

    private void OnCardOGSelected(BaseCardHUD oG)
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

    private bool IsAllyCard(BaseCardHUD card)
    {
        return !card.CompareTag("EnemyCard");
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
    private IEnumerator MoveCard(BaseCardHUD baseCardOG, Vector2 target, float duration)
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
