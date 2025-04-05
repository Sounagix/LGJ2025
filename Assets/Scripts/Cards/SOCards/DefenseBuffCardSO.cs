using UnityEngine;

[CreateAssetMenu(fileName = "NewDefenseBuffCard", menuName = "Cards/DefenseBuffCard")]
public class DefenseBuffCardSO : SupportCardSO
{
    [SerializeField]
    public int _defenseBuffValue;
}
