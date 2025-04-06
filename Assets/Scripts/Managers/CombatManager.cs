using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class CombatActions
{
    public static Action<BaseCardHUD> OnCardOGSelected;

    public static Action<SupportCardHUD> OnDropSupportCard;

    public static Action<CombatCardHUD> OnCombatCardDroped;
}

public enum ATTACK_TYPE : int
{
    ALLY,
    ENEMY
}


public enum ATTACK_STATEGY : int
{
    RANDOM,
    MAIN_CHARACTER,
    MOST_STRONGER,
    MOST_WEAK,
    SIZE
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

    [SerializeField]
    private int _minImproveHp, _minImproveAttack, _minImproveDef;

    [SerializeField]
    private int _maxImproveHp, _maxImproveAttack, _maxImproveDef;

    private Coroutine _startCoroutine = null;

    private BaseCardHUD _currentCardSelected;

    private BaseCardHUD _currentTargetSelected;

    private MapCard _mapCard;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _rewardPanel;

    [SerializeField]
    private AudioClip _selectionClip;

    [SerializeField]
    private AudioClip _attackClip;

    [SerializeField]
    private AudioClip _defeatClip;

    [SerializeField]
    private AudioClip _winClip;

    private ATTACK_STATEGY aTTACK_sTATEGY;

    private void OnEnable()
    {
        CombatActions.OnCardOGSelected += OnCardOGSelected;
        CombatActions.OnCombatCardDroped += OnCombatCardDroped;
        CombatActions.OnDropSupportCard += OnDropSupportCard;
        GameManagerActions.OnGameStateChange?.Invoke(GAME_STATE.COMBAT_STATE);
        _startCoroutine = StartCoroutine(WaitForStart());
    }

    private void OnDisable()
    {
        CombatActions.OnCombatCardDroped -= OnCombatCardDroped;
        CombatActions.OnCardOGSelected -= OnCardOGSelected;
        CombatActions.OnDropSupportCard -= OnDropSupportCard;
        GameManagerActions.OnGameStateChange?.Invoke(GAME_STATE.MAP_STATE);
    }


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
        {
            main = _mainCardTest;
            Player.Instance.AddMainCard(main);
        }
        _playerCardSlotManager.CreateMainCard(main);
    }
    private void CreateEnemies()
    {
        _enemyCardSlotManager.CreateDeckCard(_mapCard.GetCards(), _mapCard.GetCardReward());
        aTTACK_sTATEGY = (ATTACK_STATEGY)UnityEngine.Random.Range(0, (int)ATTACK_STATEGY.SIZE);
        print("STRATEGY " + aTTACK_sTATEGY);
    }

    private CombatCardHUD OnDropCardOverCombatCard(BaseCardHUD droppedHUD)
    {
        RectTransform cardRT = droppedHUD.GetComponent<RectTransform>();
        List<RectTransform> slotsRT = _playerCardSlotManager.GetCardsOnGame();
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

        return finded ? slotsRT[cont].GetComponent<CombatCardHUD>() : null;
    }

    private GameObject OnDropCardOverSlot(BaseCardHUD droppedHUD)
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

        return finded ? slotsRT[cont].gameObject : null;
    }


    private void OnCombatCardDroped(CombatCardHUD combatCardHUD)
    {
        GameObject slot = OnDropCardOverSlot(combatCardHUD);
        if (_playerCardSlotManager.IsSlotFree(slot))
        {
            _playerCardSlotManager.SetSlotAsUsed(combatCardHUD, slot);
            combatCardHUD.SetCardOnSlot(slot.transform);
        }
        else
        {
            combatCardHUD.NotSelectedOnSlot();
        }


    }

    private void OnDropSupportCard(SupportCardHUD hUD)
    {
        CombatCardHUD overlapCard = OnDropCardOverCombatCard(hUD);
        if (overlapCard != null)
        {
            if (hUD.GetHUDCardType().Equals(CARD_HUD_TYPE.HEALING))
            {
                overlapCard.HealCard((hUD as HealingCardHUD).GetHealingValue());
            }
            else if (hUD.GetHUDCardType().Equals(CARD_HUD_TYPE.ATTACK_BUFF))
            {
                overlapCard.BuffAttack((hUD as AttackBuffCardHUD).GetAttackBuffValue());
            }
            else if (hUD.GetHUDCardType().Equals(CARD_HUD_TYPE.DEFENSE_BUFF))
            {
                overlapCard.BuffDefense((hUD as DefenseBuffCardHUD).GetDefenseValue());
            }

            hUD.NotSelectedOnSlot();
            Destroy(hUD.gameObject);
        }
        else
        {
            hUD.NotSelectedOnSlot();
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
        _audioSource.PlayOneShot(_selectionClip);
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
                _currentTargetSelected.OnSelectCard();
                AttackEnemyCard();
            }
        }
        else if (_currentCardSelected)
        {
            _currentCardSelected.UnSelecCard();
            _currentCardSelected = oG;
            _currentCardSelected.OnSelectCard();
        }
        else
        {
            _currentCardSelected = oG;
            _currentCardSelected.OnSelectCard();
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
        _audioSource.PlayOneShot(_attackClip);
    }

    private void OnBackFromAttack()
    {
        if (_currentCardSelected)
            _currentCardSelected.UnSelecCard();
        if (_currentTargetSelected)
            _currentTargetSelected.UnSelecCard();
        _currentCardSelected = null;
        _currentTargetSelected = null;
        if (_enemyCardSlotManager.HaveCards())
        {
            PrepareEnemyAttack();
        }
        else
        {
            _audioSource.PlayOneShot(_winClip);
            OnFinishCombat();
        }
    }

    private IEnumerator MoveCard(BaseCardHUD attacker, BaseCardHUD defender, float duration)
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

        attacker.transform.position = target; // aseg?rate de terminar exactamente en el destino

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
        if (_playerCardSlotManager.MainCardAlive())
        {
            BaseCardHUD attacker = null;
            BaseCardHUD defender = null;
            switch (aTTACK_sTATEGY)
            {
                case ATTACK_STATEGY.RANDOM:
                    attacker = _enemyCardSlotManager.GetRandomCard();
                    defender = _playerCardSlotManager.GetRandomCard();
                    break;
                case ATTACK_STATEGY.MAIN_CHARACTER:
                    attacker = _enemyCardSlotManager.GetStrongerCard();
                    defender = _playerCardSlotManager.GetMainCard();
                    break;
                case ATTACK_STATEGY.MOST_STRONGER:
                    attacker = _enemyCardSlotManager.GetStrongerCard();
                    defender = _playerCardSlotManager.GetWeakerHP();
                    break;
                case ATTACK_STATEGY.MOST_WEAK:
                    attacker = _enemyCardSlotManager.GetStrongerCard();
                    defender = _playerCardSlotManager.GetWeakerDefense();
                    break;
            }
            StartCoroutine(MoveEnemyCard(attacker, defender, _timeToStart));
        }
        else
        {
            GameManager.Instance.LoadScene(SCENES.GAME_OVER);
        }
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

        attacker.transform.position = target; // aseg?rate de terminar exactamente en el destino

        // Aplicar da?os
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
        _audioSource.PlayOneShot(_attackClip);
        OnEnemyAttackFinish();
    }

    private void OnEnemyAttackFinish()
    {
        _playerCardSlotManager.UnblockSelection();
        _enemyCardSlotManager.UnblockSelection();
        if (!_playerCardSlotManager.MainCardAlive())
        {
            GameManager.Instance.LoadScene(SCENES.GAME_OVER);
        }
    }

    private void ApplicateDamage(BaseCardHUD attacker, BaseCardHUD defender, ATTACK_TYPE aTTACK_TYPE)
    {
        CombatCardHUD combatCardHUD = attacker.GetComponent<CombatCardHUD>();
        CombatCardHUD defederHUD = defender.GetComponent<CombatCardHUD>();

        int attackDamage = combatCardHUD.GetDamage();
        int defPoints = defederHUD.GetDefensePoints();
        bool kill = false;
        if (attackDamage - defPoints > 0)
            kill = defederHUD.SubtractLife(attackDamage - defPoints);
        else
            kill = defederHUD.SubtractLife(1);

        if (kill)
        {
            LevelUpCard(attacker as CombatCardHUD);
            if (aTTACK_TYPE.Equals(ATTACK_TYPE.ALLY))
            {
                _enemyCardSlotManager.KillCard(defederHUD);
            }
            else
            {
                _deckManager.RemoveCard(defender);
                _playerCardSlotManager.KillCard(defederHUD);
            }

            _audioSource.PlayOneShot(_defeatClip);
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

        if (_enemyCardSlotManager.GetCardType().Equals(REWARD_TYPE.BOSS))
        {
            GameManager.Instance.LoadScene(SCENES.WIN_SCENE);
        }
        else
            _rewardPanel.SetActive(true);
    }

    public void SetEnemyCards(MapCard mapCard)
    {
        _mapCard = mapCard;
    }

    private void LevelUpCard(CombatCardHUD card)
    {
        int hpValue = UnityEngine.Random.Range(_minImproveHp, _maxImproveHp);
        int atkValue = UnityEngine.Random.Range(_minImproveAttack, _maxImproveAttack);
        int defValue = UnityEngine.Random.Range(_minImproveDef, _maxImproveDef);

        card.LevelUp(hpValue, atkValue, defValue);
    }
}
