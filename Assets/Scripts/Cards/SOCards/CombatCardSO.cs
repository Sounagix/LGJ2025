using UnityEngine;

[CreateAssetMenu(fileName = "NewCombatCard", menuName = "Cards/CombatCard")]
public class CombatCardSO : BaseCardSO
{
    [SerializeField]
    public int _damage;

    [SerializeField]
    public int _block;

    [SerializeField]
    public int _life;
}
