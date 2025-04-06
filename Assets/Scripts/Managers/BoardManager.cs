using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private int _collumns;

    [SerializeField]
    private int _rows;

    [SerializeField]
    private GameObject _cardPrefab;

    [SerializeField]
    private float _spacingX;

    [SerializeField]
    private float _spacingY;

    [SerializeField]
    private float _displacementDuration;

    [SerializeField]
    private Transform _playerBody;

    private MapCard _playerCard;

    private List<MapCard> _cards = new List<MapCard>();

    private Vector2 position;

    private Coroutine _playerMovement;

    public static BoardManager Instance;

    [SerializeField]
    private GameObject _rewardPanel;

    [SerializeField]
    private GameObject _combatPanel;

    [SerializeField]
    private CombatManager _combatManager;

    private int _currentNumOfMapCardsPlayed = 0;

    public AudioSource charmoveSFX;


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

    private void Start()
    {
        GameManagerActions.OnGameStateChange?.Invoke(GAME_STATE.MAP_STATE);
        CreateBoard();
    }

    public int GetNumOfCardsTouched()
    {
        return _currentNumOfMapCardsPlayed;
    }

    private void CreateBoard()
    {
        Vector2 topLeftWorld = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 cardSize = _cardPrefab.GetComponent<SpriteRenderer>().bounds.size;
        float offsetX = cardSize.x + _spacingX;
        float offsetY = cardSize.y + _spacingY;

        for (int i = 0; i < _collumns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                Vector2 pos = new Vector2(
                    topLeftWorld.x + offsetX * i + cardSize.x / 2,
                    topLeftWorld.y + offsetY * j + cardSize.y / 2
                );

                GameObject card = Instantiate(_cardPrefab, pos, Quaternion.identity, transform);
                MapCard mapCard = card.GetComponent<MapCard>();
                mapCard.SetUp(this, new Vector2Int(i, j));
                _cards.Add(mapCard);
            }
        }
        SetPlayerOnTheCenter();

    }

    private void SetPlayerOnTheCenter()
    {
        int centerX = _collumns / 2;
        int centerY = _rows / 2;
        int centerIndex = centerY * _collumns + centerX;

        if (centerIndex >= 0 && centerIndex < _cards.Count)
        {
            _playerBody.position = _cards[centerIndex].transform.position;
            _playerCard = _cards[centerIndex];
            OpenFirstMapCard(_cards[centerIndex]);
        }
    }
    private IEnumerator MovePlayer(Vector2 target, float duration)
    {
        float elapsed = 0f;
        Vector2 start = _playerBody.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            _playerBody.position = Vector2.Lerp(start, target, t);
            yield return null;
        }
 
        _playerBody.transform.position = target;
        OnPlayerMovementFinished();
    }


    public void OnCardTouched(MapCard mapCard)
    {
        if (_playerMovement == null && IsAdjacent(_playerCard.GetIndex(), mapCard.GetIndex()))
        {
            _playerCard.OnCardUnSelected();
            _playerMovement = StartCoroutine(MovePlayer(mapCard.transform.position, _displacementDuration));
            _playerCard = mapCard;
            _playerCard.OnCardSelected();
        }
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        return (dx <= 1 && dy <= 1) && !(dx == 0 && dy == 0);
    }

    public void OnPlayerMovementFinished()
    {
        _playerMovement = null;
        if (_playerCard.IsUsed()) return;
        switch (_playerCard.GetCardType())
        {
            case CARD_TYPE.ENEMY:
                _currentNumOfMapCardsPlayed++;
                _combatPanel.SetActive(true);
                _playerCard.SetUpCards(_currentNumOfMapCardsPlayed);
                _combatManager.SetEnemyCards(_playerCard);
                break;
            case CARD_TYPE.REWARD:
                _rewardPanel.SetActive(true);
                break;
            case CARD_TYPE.BLOCK:
                break;
            case CARD_TYPE.NULL:
                break;
        }
        _playerCard.ChangeToBlock();
        charmoveSFX.Play();
    }

    private void LoadSceneMode()
    {
        Vector2 topLeftWorld = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 cardSize = _cardPrefab.GetComponent<SpriteRenderer>().bounds.size;
        float offsetX = cardSize.x + _spacingX;
        float offsetY = cardSize.y + _spacingY;

        for (int i = 0; i < _collumns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                Vector2 pos = new Vector2(
                    topLeftWorld.x + offsetX * i + cardSize.x / 2,
                    topLeftWorld.y + offsetY * j + cardSize.y / 2
                );

                GameObject card = Instantiate(_cardPrefab, pos, Quaternion.identity, transform);
                MapCard mapCard = card.GetComponent<MapCard>();
                mapCard.SetUp(this, new Vector2Int(i, j));
                _cards.Add(mapCard);
            }
        }
        SetPlayerOnTheCenter();
    }

    private void OpenFirstMapCard(MapCard card)
    {
        WaitForEnterAtFirstCard(card);
    }

    private void WaitForEnterAtFirstCard(MapCard card)
    {
        _playerCard = card;
        //_playerMovement = StartCoroutine(MovePlayer(card.transform.position, _displacementDuration));
        _playerCard.OnCardSelected();
        _rewardPanel.SetActive(true);
    }
}
