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
            if (player.targetDistance <= player.AttackRange)
            {
                player.SetIsAttackFinishing(false);
                player.SetIsAttacking(true);
                ExecuteAttack();
            }

            return;
        }

        if (!player.targetEnemy)
        {
            player.animator.ResetTrigger(AnimHash.Attack);
            player.SetIsAttacking(false);
            return;
        }

        if (player.targetDistance <= player.AttackRange)
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
            player.animator.SetLayerWeight(AnimHash.FullBodyLayer, Mathf.Lerp(player.animator.GetLayerWeight(AnimHash.FullBodyLayer), 0.0f, speed));
            player.animator.SetLayerWeight(AnimHash.UpperBodyLayer, Mathf.Lerp(player.animator.GetLayerWeight(AnimHash.UpperBodyLayer), 0.0f, speed));

            if (player.animator.GetLayerWeight(AnimHash.FullBodyLayer) < 0.01f && player.animator.GetLayerWeight(AnimHash.UpperBodyLayer) < 0.01f)
            {
                player.animator.SetLayerWeight(AnimHash.FullBodyLayer, 0f);
                player.animator.SetLayerWeight(AnimHash.UpperBodyLayer, 0f);
                player.SetIsAttackFinishing(false);
                player.SetIsAttacking(false);
            }
            return;
        }

        if (player.isAttacking)
        {
            bool isMoving = player.move.MoveDirection.sqrMagnitude > 0.1f;

            if (isMoving)
            {
                player.animator.SetLayerWeight(AnimHash.FullBodyLayer, Mathf.Lerp(player.animator.GetLayerWeight(AnimHash.FullBodyLayer), 0.0f, speed));
                player.animator.SetLayerWeight(AnimHash.UpperBodyLayer, Mathf.Lerp(player.animator.GetLayerWeight(AnimHash.UpperBodyLayer), 1.0f, speed));
            }
            else
            {
                player.animator.SetLayerWeight(AnimHash.FullBodyLayer, Mathf.Lerp(player.animator.GetLayerWeight(AnimHash.FullBodyLayer), 1.0f, speed));
                player.animator.SetLayerWeight(AnimHash.UpperBodyLayer, Mathf.Lerp(player.animator.GetLayerWeight(AnimHash.UpperBodyLayer), 0.0f, speed));
            }
        }
        else
        {
            if (!player.isAttackFinishing)
            {
                player.animator.SetLayerWeight(AnimHash.FullBodyLayer, Mathf.Lerp(player.animator.GetLayerWeight(AnimHash.FullBodyLayer), 0.0f, speed));
                player.animator.SetLayerWeight(AnimHash.UpperBodyLayer, Mathf.Lerp(player.animator.GetLayerWeight(AnimHash.UpperBodyLayer), 0.0f, speed));
            }
        }
    }
    public virtual bool OnTargetLost()
    {
        player.SetIsAttackFinishing(true);
        player.animator.ResetTrigger(AnimHash.Attack);
        return true;
    }

    public virtual void ExecuteAttack()
    {
        player.animator.SetTrigger(AnimHash.Attack);
    }
}