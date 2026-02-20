using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
    public EnemyHitState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
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

    }
}
