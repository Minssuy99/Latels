using UnityEngine;

public class ShinanoAttack : PlayerAttack, IBattleComponent
{
    [SerializeField] private GameObject hitbox;

    public override void ExecuteAttack()
    {
        base.ExecuteAttack();
    }

    public override void EnableHitbox()
    {
        hitbox.SetActive(true);
    }

    public override void DisableHitbox()
    {
        hitbox.SetActive(false);
    }
}