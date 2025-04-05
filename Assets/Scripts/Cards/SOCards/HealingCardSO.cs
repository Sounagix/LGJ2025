using UnityEngine;

[CreateAssetMenu(fileName = "NewHealingCard", menuName = "Cards/HealingCard")]
public class HealingCardSO : SupportCardSO
{
    [SerializeField]
    public int _valueOfHealing;
}
