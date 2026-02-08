public class EnemyHitState : EnemyBaseState
{
    public EnemyHitState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.animator.SetTrigger("Hit");
        enemy.attack.DiableAllHitboxes();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
