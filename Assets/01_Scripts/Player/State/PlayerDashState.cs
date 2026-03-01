public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.SetCanAttack(false);
        player.SetIsLockedOn(false);
        player.animator.SetBool(AnimHash.IsLockedOn, false);
        player.SetIsAttacking(false);
        player.animator.applyRootMotion = false;

        player.animator.SetLayerWeight(1, 0f);
        player.animator.SetLayerWeight(2, 0f);
        player.animator.SetTrigger(AnimHash.Dash);
        player.StartCoroutine(player.dash.DashCoroutine());
    }

    public override void Update()
    {
        player.animator.SetFloat(AnimHash.Velocity, player.move.MoveDirection.magnitude);
    }

    public override void Exit()
    {
        player.animator.applyRootMotion = true;
    }
}