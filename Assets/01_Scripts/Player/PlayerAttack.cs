 using Unity.VisualScripting;
 using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    protected PlayerStateManager player;

    protected virtual void Awake()
    {
        player = GetComponent<PlayerStateManager>();
    }

    protected virtual void Update()
    {
        if (player.IsSprinting) return;
        if (player.IsDashing) return;
        if (!player.canAttack) return;
        if (player.IsUsingSkill) return;

        UpdateAttack();
    }

    private void UpdateAttack()
    {
        if (player.isAttackFinishing)
        {
            if (player.targetDistance <= player.CharacterData.stats.attackRange)
            {
                player.SetIsAttackFinishing(false);
                player.SetIsAttacking(true);
                ExecuteAttack();
            }

            return;
        }

        if (!player.targetEnemy)
        {
            player.animator.ResetTrigger("Attack");
            player.SetIsAttacking(false);
            return;
        }

        if (player.targetDistance <= player.CharacterData.stats.attackRange)
        {
            player.SetIsAttacking(true);
            ExecuteAttack();
        }
        else
        {
            if (player.isAttacking)
            {
                OnTargetLost();
            }
            player.SetIsAttacking(false);
        }
    }
    public virtual void UpdateAttackLayers()
    {
        float speed = 10f * TimeManager.Instance.PlayerDelta;

        if (player.isAttackFinishing)
        {
            player.animator.SetLayerWeight(1, Mathf.Lerp(player.animator.GetLayerWeight(1), 0.0f, speed));
            player.animator.SetLayerWeight(2, Mathf.Lerp(player.animator.GetLayerWeight(2), 0.0f, speed));

            if (player.animator.GetLayerWeight(1) < 0.01f && player.animator.GetLayerWeight(2) < 0.01f)
            {
                player.animator.SetLayerWeight(1, 0f);
                player.animator.SetLayerWeight(2, 0f);
                player.SetIsAttackFinishing(false);
                player.SetIsAttacking(false);
            }
            return;
        }

        if (player.isAttacking)
        {
            bool isMoving = player.move.moveDirection.sqrMagnitude > 0.1f;

            if (isMoving)
            {
                player.animator.SetLayerWeight(1, Mathf.Lerp(player.animator.GetLayerWeight(1), 0.0f, speed));
                player.animator.SetLayerWeight(2, Mathf.Lerp(player.animator.GetLayerWeight(2), 1.0f, speed));
            }
            else
            {
                player.animator.SetLayerWeight(1, Mathf.Lerp(player.animator.GetLayerWeight(1), 1.0f, speed));
                player.animator.SetLayerWeight(2, Mathf.Lerp(player.animator.GetLayerWeight(2), 0.0f, speed));
            }
        }
        else
        {
            if (!player.isAttackFinishing)
            {
                player.animator.SetLayerWeight(1, Mathf.Lerp(player.animator.GetLayerWeight(1), 0.0f, speed));
                player.animator.SetLayerWeight(2, Mathf.Lerp(player.animator.GetLayerWeight(2), 0.0f, speed));
            }
        }
    }
    public virtual bool OnTargetLost()
    {
        player.SetIsAttackFinishing(true);
        player.animator.ResetTrigger("Attack");
        return true;
    }

    public virtual void ExecuteAttack()
    {
        player.animator.SetTrigger("Attack");
    }

    private void OnDrawGizmosSelected()
    {
        if (player.CharacterData == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, player.CharacterData.stats.attackRange);

        if (player == null || player.targetEnemy == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.targetEnemy.transform.position);
    }
}