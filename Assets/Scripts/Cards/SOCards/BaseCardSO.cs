using UnityEngine;


public class BaseCardSO : ScriptableObject
{
    [SerializeField]
    public string _cardName;

    [SerializeField]
    public string _cardDescription;

    [SerializeField]
    public Sprite _cardImage;

    [SerializeField]
    public CARD_HUD_TYPE cARD;

    [SerializeField]
    public REWARD_TYPE rEWARD_tYPE;
}
