using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CombatActions
{
    public static Action<BaseCardHUD> OnDropDragedCard;

    public static Action<BaseCardHUD> OnCardOGSelected;
} 

public enum ATTACK_TYPE : int
{
    ALLY,
    ENEMY
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
    private float _enemyTimeToWaitForAttack;

    [SerializeField]
    private BaseCardSO _mainCardTest;

    [SerializeField]
    private DeckManager _deckManager;

    private Coroutine _startCoroutine = null;

    private BaseCardHUD _currentCardSelected;

    private BaseCardHUD _currentTargetSelected;

    private MapCard _mapCard;

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
        _enemyCardSlotManager.CreateDeckCard(_mapCard.GetCards());
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
            _playerCardSlotManager.SetSlotAsUsed(droppedHUD, cont);
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
        _playerCardSlotManager.BlockSelection();
        _enemyCardSlotManager.BlockSelection();
        StartCoroutine(MoveCard(_currentCardSelected, _currentTargetSelected, _timeToAttack));
    }
    
    private void OnBackFromAttack()
    {
        _currentCardSelected.UnSelecCard();
        _currentTargetSelected.UnSelecCard();
        _currentCardSelected = null;
        _currentTargetSelected = null;
        if (_enemyCardSlotManager.HaveCards())
        {
            PrepareEnemyAttack();
        }
        else
        {
            OnFinishCombat();
        }
    }

    private IEnumerator MoveCard(BaseCardHUD attacker,BaseCardHUD defender , float duration)
    {
        Vector2 startPos = attacker.transform.position;
        Vector2 target = defender.transform.position;
        float elapsed = 0f;
    
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            attacker.transform.position = Vector2.Lerp(startPos, target, t);
            yield return null;
        }
    
        attacker.transform.position = target; // asegúrate de terminar exactamente en el destino

        ApplicateDamage(attacker, defender, ATTACK_TYPE.ALLY);

        startPos = attacker.transform.position;
        target = attacker.transform.parent.transform.position;
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            attacker.transform.position = Vector2.Lerp(startPos, target, t);
            yield return null;
        }
        attacker.transform.position = target;
        OnBackFromAttack();
    }

    private void PrepareEnemyAttack()
    {
        BaseCardHUD enemyCardSelected = SelectStrategy();
        BaseCardHUD cardToAttack = SelectCardToAttack();
        StartCoroutine(MoveEnemyCard(enemyCardSelected, cardToAttack, _timeToStart));
    }

    private BaseCardHUD SelectStrategy()
    {
        return _enemyCardSlotManager.GetRandomCard();
    }

    private BaseCardHUD SelectCardToAttack()
    {
        return _playerCardSlotManager.GetRandomCard();
    }

    private IEnumerator MoveEnemyCard(BaseCardHUD attacker, BaseCardHUD defender, float duration)
    {
        yield return new WaitForSecondsRealtime(_enemyTimeToWaitForAttack);
        Vector2 target = defender.transform.position;
        Vector2 startPos = attacker.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            attacker.transform.position = Vector2.Lerp(startPos, target, t);
            yield return null;
        }

        attacker.transform.position = target; // asegúrate de terminar exactamente en el destino

        // Aplicar daños
        ApplicateDamage(attacker, defender, ATTACK_TYPE.ENEMY);

        startPos = attacker.transform.position;
        target = attacker.transform.parent.transform.position;
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            attacker.transform.position = Vector2.Lerp(startPos, target, t);
            yield return null;
        }
        attacker.transform.position = target;
        OnEnemyAttackFinish();
    }

    private void OnEnemyAttackFinish()
    {
        _playerCardSlotManager.UnblockSelection();
        _enemyCardSlotManager.UnblockSelection();
        if (!_playerCardSlotManager.HaveCards())
        {
            // TODO GameOver status
            GameManager.Instance.LoadScene(SCENES.MAIN_MENU);
        }
    }

    private void ApplicateDamage(BaseCardHUD attacker, BaseCardHUD defender, ATTACK_TYPE aTTACK_TYPE)
    {
        int attackDamage = attacker.GetDamage();
        int defPoints = defender.GetDefensePoints();
        bool kill = false;
        if (attackDamage - defPoints > 0)
            kill = defender.SubtractLife(attackDamage - defPoints);

        if (kill)
        {
            if (aTTACK_TYPE.Equals(ATTACK_TYPE.ALLY))
            {
                _enemyCardSlotManager.KillCard(defender);
            }
            else
            {
                _playerCardSlotManager.KillCard(defender);
                _deckManager.RemoveCard(defender);
            }
        }
    }

    private void OnFinishCombat()
    {
        _currentCardSelected = null;
        _currentTargetSelected = null;
        _playerCardSlotManager.Clean();
        _enemyCardSlotManager.Clean();
        _handManager.Clean();
        gameObject.SetActive(false);
        _deckManager.Clean();
    }

    public void SetEnemyCards(MapCard mapCard)
    {
        _mapCard = mapCard;
    }
}
