public class EnemyInactiveState : EnemyBaseState
{
    public EnemyInactiveState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        if (enemy.playerState.IsDead)
        {
            enemy.animator.SetBool("isRunning", false);
            enemy.animator.SetBool("isReady", false);
            enemy.agent.isStopped = true;
            enemy.transform.LookAt(enemy.playerPos);
        }
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}