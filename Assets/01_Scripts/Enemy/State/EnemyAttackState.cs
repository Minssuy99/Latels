using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.agent.updatePosition = false;
        enemy.attack.attackType = Random.Range(1, 3);
        enemy.animator.SetInteger("AttackType", enemy.attack.attackType);
        enemy.animator.SetTrigger("Attack");
    }

    public override void Update()
    {
        if (Time.timeScale < 1f)
        {
            enemy.rotationLocked = true;
        }

        if (!enemy.rotationLocked)
        {
            enemy.transform.LookAt(enemy.playerPos);
        }
    }

    public override void Exit()
    {
        enemy.agent.Warp(enemy.transform.position);
        enemy.agent.updatePosition = true;
        enemy.attack.attackCooldown = Random.Range(1, enemy.Data.stats.attackCooldown);
        enemy.rotationLocked = false;
        enemy.attack.superArmor = false;
        enemy.health.hitCount = 0;
    }
}