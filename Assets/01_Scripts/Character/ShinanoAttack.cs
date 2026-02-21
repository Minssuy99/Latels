using UnityEngine;

public class ShinanoAttack : PlayerAttack, IBattleComponent
{
    [SerializeField] private GameObject hitbox;

    public void EnableHitbox()
    {
        hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitbox.SetActive(false);
    }
}