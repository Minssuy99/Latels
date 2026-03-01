using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.agent.updatePosition = false;
        enemy.attack.attackType = Random.Range(0, enemy.Data.stats.attackTypeCount);
        enemy.animator.SetInteger(AnimHash.AttackType, enemy.attack.attackType);
        enemy.animator.SetTrigger(AnimHash.Attack);
    }

    public override void Update()
    {
        if (TimeManager.Instance.IsSlowMotion)
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