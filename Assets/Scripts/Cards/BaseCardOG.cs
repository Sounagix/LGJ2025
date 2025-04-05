using TMPro;
using UnityEngine;

/// <summary>
/// Card used to represent a card in the gameplay scene
/// </summary>
public class BaseCardOG : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _title;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private bool _selected = false;

    private Vector2 _initPos;

    private void Start()
    {
        _initPos = transform.position;
    }

    public void Initialize(BaseCardSO card)
    {
        if (!card) return;
        _title.text = card._cardName;
        _spriteRenderer.sprite = card._cardImage;
    }

    public void Initialize(BaseCardHUD hudCard)
    {
        _title.text = hudCard.GetName();
        _spriteRenderer.sprite = hudCard.GetSpriteFromRawImage();
    }

    private void OnMouseUpAsButton()
    {
        _selected = true;
        _spriteRenderer.material.color = Color.gray;
        //CombatActions.OnCardOGSelected?.Invoke(this);
        transform.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    private void OnMouseOver()
    {
        if (!_selected)
            transform.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    private void OnMouseExit()
    {
        if (!_selected)
        {
            transform.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.position = _initPos;
        }
    }

    public void UnSelecCard()
    {
        _selected = false;
        _spriteRenderer.material.color = Color.white;
        transform.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
