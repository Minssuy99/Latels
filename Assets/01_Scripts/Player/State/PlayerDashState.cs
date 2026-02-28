public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.SetCanAttack(false);
        player.SetIsAttacking(false);

        player.animator.SetLayerWeight(1, 0f);
        player.animator.SetLayerWeight(2, 0f);
        player.animator.SetTrigger("Dash");
        player.StartCoroutine(player.dash.DashCoroutine());
    }

    public override void Update()
    {
        player.animator.SetFloat("Velocity", player.move.moveDirection.magnitude);
    }

    public override void Exit()
    {

    }
}