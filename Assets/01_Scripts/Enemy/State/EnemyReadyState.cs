public class EnemyReadyState : EnemyBaseState
{
    public EnemyReadyState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.animator.SetBool(AnimHash.IsReady, true);
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}