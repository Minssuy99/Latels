using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.area.RemoveEnemy(enemy);
        enemy.animator.SetTrigger("Die");
        enemy.transform.LookAt(enemy.player.transform);
        enemy.StartCoroutine(enemy.SinkAndDestroy());

        enemy.attack.capsuleCollider.enabled = false;
        enemy.attack.enabled = false;
        enemy.agent.enabled = false;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}