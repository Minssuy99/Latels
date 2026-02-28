using UnityEngine;

public class LockOnController : MonoBehaviour, IBattleComponent
{
    private PlayerStateManager player;

    private void Awake()
    {
        player = GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        if (player.IsSprinting) return;
        if (player.IsDashing) return;
        if (player.IsUsingSkill) return;

        player.targetEnemy = player.targetDetector.FindNearestTarget();
        player.targetDistance = player.targetDetector.LastDistance;

        UpdateLockOn();
    }

    private void UpdateLockOn()
    {
        if (player.targetEnemy)
        {
            if (player.targetDistance <= player.CharacterData.stats.attackRange)
            {
                player.SetIsLockedOn(true);
                player.animator.SetBool("isLockedOn", true);
            }
            else
            {
                if (player.isLockedOn)
                {
                    bool releaseLockOn = true;

                    if (player.isAttacking || player.isAttackFinishing)
                    {
                        releaseLockOn = player.attack.OnTargetLost();
                    }

                    if (releaseLockOn)
                    {
                        player.animator.SetBool("isLockedOn", false);
                        player.SetIsLockedOn(false);
                    }
                }
            }
        }
        else
        {
            bool releaseLockOn = true;

            if (player.isAttacking || player.isAttackFinishing)
            {
                releaseLockOn = player.attack.OnTargetLost();
            }

            if (releaseLockOn)
            {
                player.SetIsLockedOn(false);
                player.animator.SetBool("isLockedOn", false);
            }
        }
    }
}