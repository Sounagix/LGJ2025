using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    private CardSlotsManager _slotsManager;


    private void Awake()
    {
        _slotsManager = GetComponent<CardSlotsManager>();
    }
}
