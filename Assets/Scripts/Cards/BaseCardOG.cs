using System.Collections;
using System.Collections.Generic;
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

    public void Initialize(BaseCardSO card)
    {
        _title.text = card._cardName;
        _spriteRenderer.sprite = card._cardImage;
    }

    private void OnMouseEnter()
    {
        OnCLickOnCard();   
    }

    private void OnMouseOver()
    {
        transform.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    private void OnMouseExit()
    {
        transform.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }


    private void OnCLickOnCard()
    {

    }
}
