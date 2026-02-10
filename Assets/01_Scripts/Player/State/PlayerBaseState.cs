public abstract class PlayerBaseState : IState
{
    protected PlayerStateManager player;

    public PlayerBaseState(PlayerStateManager player)
    {
        this.player = player;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}