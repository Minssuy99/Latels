using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
    public EnemyHitState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.agent.updatePosition = false;
        if (Time.timeScale > 0.1f)
        {
            enemy.transform.LookAt(enemy.playerPos);
        }

        enemy.animator.SetTrigger("Hit");
        enemy.attack.DisableAllHitboxes();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        enemy.agent.Warp(enemy.transform.position);
        enemy.agent.updatePosition = true;
    }
}
