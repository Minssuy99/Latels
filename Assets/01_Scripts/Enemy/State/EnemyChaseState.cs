using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.agent.isStopped = false;
        enemy.animator.SetBool("isRunning", true);
    }

    public override void Update()
    {
        enemy.targetDistance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        if (Time.timeScale > 0.1f)
        {
            if (!enemy.rotationLocked)
            {
                enemy.transform.LookAt(enemy.player.transform);
            }
        }

        if (enemy.targetDistance <= enemy.agent.stoppingDistance)
        {
            enemy.agent.isStopped = true;
            enemy.animator.SetBool("isRunning", false);

            enemy.attack.attackCooldown -= TimeManager.Instance.EnemyDelta;

            if (enemy.attack.attackCooldown <= 0)
            {
                enemy.ChangeState(enemy.attackState);
            }

        }
        else
        {
            enemy.agent.isStopped = false;
            enemy.agent.SetDestination(enemy.player.transform.position);
            enemy.animator.SetBool("isRunning", true);
        }
    }

    public override void Exit()
    {
        enemy.agent.isStopped = true;
        enemy.animator.SetBool("isRunning", false);
    }
}