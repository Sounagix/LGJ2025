using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCombatCard", menuName = "Cards/CombatCard")]
public class CombatCardSO : BaseCardSO
{
    [SerializeField]
    private int _damage;

    [SerializeField]
    private int _block;

    [SerializeField]
    private int _life;
}
