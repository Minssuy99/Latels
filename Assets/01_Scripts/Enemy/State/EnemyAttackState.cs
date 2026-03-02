using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.agent.updatePosition = false;
        enemy.attack.AttackType = Random.Range(0, enemy.AttackTypeCount);
        enemy.animator.SetInteger(AnimHash.AttackType, enemy.attack.AttackType);
        enemy.animator.SetTrigger(AnimHash.Attack);
    }

    public override void Update()
    {
        if (TimeManager.Instance.IsSlowMotion)
        {
            enemy.rotationLocked = true;
            enemy.attack.DisableAllHitboxes();
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
        enemy.attack.ResetAfterAttack();
        enemy.health.ResetHitCount();
        enemy.rotationLocked = false;
    }
}