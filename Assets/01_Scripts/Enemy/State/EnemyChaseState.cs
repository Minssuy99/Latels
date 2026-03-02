using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.agent.isStopped = false;
        enemy.animator.SetBool(AnimHash.IsRunning, true);
    }

    public override void Update()
    {
        enemy.targetDistance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        if (!TimeManager.Instance.IsSlowMotion)
        {
            if (!enemy.rotationLocked)
            {
                enemy.transform.LookAt(enemy.playerPos);
            }
        }

        if (enemy.targetDistance <= enemy.agent.stoppingDistance)
        {
            enemy.agent.isStopped = true;
            enemy.animator.SetBool(AnimHash.IsRunning, false);

            enemy.attack.TickCooldown();

            if (enemy.attack.IsReady)
            {
                enemy.ChangeState(enemy.attackState);
            }

        }
        else
        {
            enemy.agent.isStopped = false;
            enemy.agent.SetDestination(enemy.player.transform.position);
            enemy.animator.SetBool(AnimHash.IsRunning, true);
        }
    }

    public override void Exit()
    {
        enemy.agent.isStopped = true;
        enemy.animator.SetBool(AnimHash.IsRunning, false);
    }
}