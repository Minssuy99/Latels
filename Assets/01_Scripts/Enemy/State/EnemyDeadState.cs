public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.area?.RemoveEnemy(enemy);
        enemy.attack.DisableAllHitboxes();
        enemy.animator.SetTrigger("Die");
        enemy.transform.LookAt(enemy.playerPos);
        enemy.StartCoroutine(enemy.SinkAndDestroy());

        enemy.health.DisableCollider();
        enemy.health.enabled = false;
        enemy.agent.enabled = false;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}