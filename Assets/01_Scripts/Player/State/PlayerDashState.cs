public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.canAttack = false;
        player.isAttacking = false;

        player.animator.SetLayerWeight(1, 0f);
        player.animator.SetLayerWeight(2, 0f);
        player.animator.SetTrigger("Dash");
        player.StartCoroutine(player.dash.DashCoroutine());
    }

    public override void Update()
    {
        player.characterController.Move(player.dash.dashDirection * (player.dash.dashSpeed * TimeManager.Instance.PlayerDelta));
        player.animator.SetFloat("Velocity", player.move.moveDirection.magnitude);
    }

    public override void Exit()
    {

    }
}