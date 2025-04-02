using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CARD_HUD_TYPE : int
{
    COMBAT,
    HEALING,
    NULL,
}

public class BaseCardHUD : MonoBehaviour, IPointerEnterHandler, 
    IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    protected TextMeshProUGUI _cardName, _cardDescription;

    [SerializeField]
    protected RawImage _image;

    protected BaseCardSO _baseCardSO;

    protected Vector2 _initialAnchoredPosition;

    protected bool blocked = false;

    protected Image _cardImg;

    protected bool _selected = false;

    protected CardSlotsManager _cardSlotsManager;

    protected bool _rdyForDestroy = false;

    protected bool _cardOnGame = false;

    protected CARD_HUD_TYPE cARD = CARD_HUD_TYPE.NULL;

    public virtual void  Initialize(BaseCardSO card, CardSlotsManager cardSlotsManager = null)
    {
        _cardSlotsManager = cardSlotsManager;
        _cardName.text = card._cardName;
        _cardDescription.text = card._cardDescription;
        _image.texture = card._cardImage.texture;
        _baseCardSO = card;
        _initialAnchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        _cardImg = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (blocked) return;
        if (!_selected)
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (blocked) return;
        if (!_selected)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (blocked || _cardOnGame) return;
        switch (GameManager.Instance.GetGAME_STATE())
        {
            case GAME_STATE.SELECTION_STATE:
                Player.Instance.AddMainCard(_baseCardSO);
                GameManager.Instance.LoadScene(SCENES.GAME);  
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

    public virtual void OnEndDrag(PointerEventData eventData)
    {
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
        _cardOnGame = true;
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

    public void KillCard()
    {
        _rdyForDestroy = true;
    }

    public CARD_HUD_TYPE GetHUDCardType()
    {
        return cARD;
    }
}
