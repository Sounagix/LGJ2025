using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackBuffCard", menuName = "Cards/AttackBuffCard")]
public class AttackBuffCardSO : SupportCardSO
{
    [SerializeField]
    public int _attackIncreased;
}
