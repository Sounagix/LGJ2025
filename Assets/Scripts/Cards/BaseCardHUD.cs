using TMPro;
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

    public void Initialize(BaseCardSO card)
    {
        _cardName.text = card._cardName;
        _cardDescription.text = card._cardDescription;
        _image.texture = card._cardImage.texture;
        _baseCardSO = card;
        _initialAnchoredPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (GameManager.Instance.GetScene())
        {
            case SCENES.INIT_SCENE:
                Player.Instance.AddMainCard(_baseCardSO);
                GameManager.Instance.LoadScene(SCENES.GAME);
                break;
            case SCENES.COMBAT:
                break;
            case SCENES.REWARD:
                Player.Instance.AddCardToDeck(_baseCardSO);
                GameManager.Instance.LoadScene(SCENES.GAME);
                break;
        }
        
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.GetScene().Equals(SCENES.COMBAT))
            transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CombatActions.OnDropDragedCard?.Invoke(this);
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
        //GetComponent<RectTransform>().localPosition = _initialAnchoredPosition;
    }
}
