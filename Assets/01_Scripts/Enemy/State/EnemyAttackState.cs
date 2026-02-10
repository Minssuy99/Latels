using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.attack.attackType = Random.Range(0, 3);
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
            enemy.transform.LookAt(enemy.player.transform);
        }
    }

    public override void Exit()
    {
        enemy.attack.attackCooldown = Random.Range(1, 3);
        enemy.rotationLocked = false;
        enemy.attack.superArmor = false;
        enemy.attack.hitCount = 0;
    }
}