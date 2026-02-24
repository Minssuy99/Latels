using UnityEngine;

public class ShinanoAttack : PlayerAttack, IBattleComponent
{
    [SerializeField] private GameObject hitbox;

    private void Start()
    {
        DisableHitbox();
    }

    public void EnableHitbox()
    {
        hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitbox.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!player.targetEnemy)
        {
            DisableHitbox();
        }
    }
}