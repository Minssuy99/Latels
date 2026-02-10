using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.isHit = false;
        player.canAttack = false;
        player.isAttacking = false;
        player.dash.canDash = false;

        player.animator.SetLayerWeight(1, 0f);
        player.animator.SetLayerWeight(2, 0f);
        player.animator.SetLayerWeight(3, 0f);
        player.animator.SetLayerWeight(4, 0f);
        player.animator.SetTrigger("Dash");
        player.StartCoroutine(player.dash.DashCoroutine());
    }

    public override void Update()
    {
        player.characterController.Move(player.dash.dashDirection * (player.dash.dashSpeed * Time.unscaledDeltaTime));
    }

    public override void Exit()
    {

    }
}