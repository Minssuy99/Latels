using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    private List<EnemyStateManager> enemies = new ();
    public List<EnemyStateManager> GetEnemies() => enemies;

    protected PlayerStateManager player;

    private void Awake()
    {
        player = GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        if (player.IsSprinting) return;
        if (player.IsDashing) return;
        if (!player.canAttack) return;
        if (player.IsUsingSkill) return;

        player.targetDistance = SelectNearestEnemy();

        UpdateLockOn();
        UpdateAttack();
    }

    private float SelectNearestEnemy()
    {
        if (player == null) return Mathf.Infinity;

        float targetDistance = Mathf.Infinity;
        player.targetEnemy = null;
        if (enemies.Count > 0)
        {
            foreach (EnemyStateManager enemy in enemies)
            {
                if (enemy == null) continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < targetDistance)
                {
                    targetDistance = distance;
                    player.targetEnemy = enemy;
                }
            }
        }
        else
        {
            player.targetEnemy = null;
        }

        return targetDistance;
    }

    private void UpdateLockOn()
    {
        if (player.targetEnemy != null)
        {
            if (player.targetDistance <= player.CharacterData.stats.lockOnRange)
            {
                player.isLockedOn = true;
                player.animator.SetBool("isLockedOn", true);
            }
            else
            {
                if (player.isLockedOn)
                {
                    if (player.isAttacking)
                    {
                        player.isAttackFinishing = true;
                        player.animator.ResetTrigger("Attack");
                    }


                    player.animator.SetBool("isLockedOn", false);
                    player.isLockedOn = false;
                }
            }
        }
        else
        {
            if (player.isAttacking)
            {
                player.isAttackFinishing = true;
                player.animator.ResetTrigger("Attack");
            }

            player.isLockedOn = false;
            player.animator.SetBool("isLockedOn", false);
        }
    }

    private void UpdateAttack()
    {
        if (player.isAttackFinishing)
        {
            if (player.targetDistance <= player.CharacterData.stats.attackRange)
            {
                player.isAttackFinishing = false;
                player.isAttacking = true;
                ExecuteAttack();
            }
            return;
        }

        if (!player.targetEnemy)
        {
            player.animator.ResetTrigger("Attack");
            player.isAttacking = false;
            return;
        }

        if (player.targetDistance <= player.CharacterData.stats.attackRange)
        {
            player.isAttacking = true;
            ExecuteAttack();
        }
        else
        {
            if (player.isAttacking)
            {
                player.isAttackFinishing = true;
                player.animator.ResetTrigger("Attack");
            }
            player.isAttacking = false;
        }
    }

    public void SetEnemies(List<EnemyStateManager> enemies)
    {
        this.enemies = enemies;
    }

    private void OnDrawGizmosSelected()
    {
        if (player.CharacterData == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, player.CharacterData.stats.lockOnRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, player.CharacterData.stats.attackRange);

        if (player == null || player.targetEnemy == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.targetEnemy.transform.position);
    }

    public virtual void ExecuteAttack()
    {
        player.animator.SetTrigger("Attack");
    }
}
