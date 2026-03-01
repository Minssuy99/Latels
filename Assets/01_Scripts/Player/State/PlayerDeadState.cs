public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.animator.SetLayerWeight(1, 0f);
        player.animator.SetLayerWeight(2, 0f);

        player.animator.SetTrigger(AnimHash.Die);

        player.SetIsAttacking(false);
        player.attack.enabled = false;
        player.health.enabled = false;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
