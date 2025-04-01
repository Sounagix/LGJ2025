using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseCardHUD : MonoBehaviour, IPointerEnterHandler, 
    IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private TextMeshProUGUI _cardName, _cardDescription;

    [SerializeField]
    private RawImage _image;

    [SerializeField]
    private Player _player;

    private BaseCardSO _baseCardSO;

    private Vector2 _initialAnchoredPosition;

    private bool blocked = false;

    private Image _cardImg;

    private bool _selected = false;

    private int _attackDamage;

    private int _lifePoints;

    private int _defensePoints;

    private CardSlotsManager _cardSlotsManager;

    private bool _rdyForDestroy = false;

    public void Initialize(BaseCardSO card, CardSlotsManager cardSlotsManager = null)
    {
        _cardSlotsManager = cardSlotsManager;
        _cardName.text = card._cardName;
        _cardDescription.text = card._cardDescription;
        _image.texture = card._cardImage.texture;
        _baseCardSO = card;
        _initialAnchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        _cardImg = GetComponent<Image>();

        CombatCardSO combatCardSO = card as CombatCardSO;
        if (combatCardSO)
        {
            _attackDamage = combatCardSO._damage;
            _lifePoints = combatCardSO._life;
            _defensePoints = combatCardSO._block;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_selected)
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_selected)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (GameManager.Instance.GetGAME_STATE())
        {
            case GAME_STATE.SELECTION_STATE:
                Player.Instance.AddMainCard(_baseCardSO);
                RewardManagerActions.OnRewardCollected?.Invoke();   
                break;
            case GAME_STATE.MAP_STATE:
                break;
            case GAME_STATE.COMBAT_STATE:
                _selected = true;
                _cardImg.color = Color.gray;
                CombatActions.OnCardOGSelected?.Invoke(this);
                break;
            case GAME_STATE.REWARD_STATE:
                Player.Instance.AddCardToDeck(_baseCardSO);
                RewardManagerActions.OnRewardCollected?.Invoke();
                break;
            case GAME_STATE.NULL:
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (blocked) return;
        GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (blocked) return;
        if (GameManager.Instance.GetGAME_STATE().Equals(GAME_STATE.COMBAT_STATE))
            transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (blocked) return;
        if (GameManager.Instance.GetGAME_STATE().Equals(GAME_STATE.COMBAT_STATE))
            CombatActions.OnDropDragedCard?.Invoke(this);
    }

    public void BlockCard()
    {
        blocked = true;
    }

    public void UnlockCard()
    {
        blocked = false;
    }

    public string GetName()
    {
        return _cardName.text;
    }

    public string GetDescription()
    {
        return _cardDescription.text;
    }

    public Sprite GetSpriteFromRawImage()
    {
        Texture2D tex = _image.texture as Texture2D;
        if (tex == null) return null;

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public void NotSelectedOnSlot()
    {
        GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
    }

    public void SetCardOnSlot(Transform tr)
    {
        transform.SetParent(tr);
        transform.localPosition = Vector3.zero;
    }

    public void UnSelecCard()
    {
        if (_rdyForDestroy)
        {
            _cardSlotsManager.DestroyCard(this);
            return;
        }
        _selected = false;
        _cardImg.color = Color.white;
        transform.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public int GetDamage()
    {
        return _attackDamage;
    }

    public int GetLifePoints()
    {
        return _lifePoints;
    }

    public int GetDefensePoints()
    {
        return _defensePoints;
    }

    public bool SubtractLife(int value)
    {
        _lifePoints -= value;
        return _lifePoints <= 0;
    }

    public void KillCard()
    {
        _rdyForDestroy = true;
    }
}
