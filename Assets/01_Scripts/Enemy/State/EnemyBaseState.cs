public abstract class EnemyBaseState : IState
{
    protected EnemyStateManager enemy;

    public EnemyBaseState(EnemyStateManager enemy)
    {
        this.enemy = enemy;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
