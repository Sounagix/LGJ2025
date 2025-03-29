using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/BaseCard")]
public class BaseCardSO : ScriptableObject
{
    [SerializeField]
    public string _cardName;

    [SerializeField]
    public string _cardDescription;

    [SerializeField]
    public Sprite _cardImage;
}
