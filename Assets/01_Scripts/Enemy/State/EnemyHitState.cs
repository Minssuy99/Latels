public class EnemyHitState : EnemyBaseState
{
    public EnemyHitState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.agent.updatePosition = false;
        if (!TimeManager.Instance.IsSlowMotion)
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
