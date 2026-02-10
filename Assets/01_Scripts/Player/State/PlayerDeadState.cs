using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.animator.SetLayerWeight(1, 0f);
        player.animator.SetLayerWeight(2, 0f);
        player.animator.SetLayerWeight(3, 0f);
        player.animator.SetLayerWeight(4, 0f);

        player.attack.DisableHitbox();
        player.animator.SetTrigger("Die");

        player.isAttacking = false;
        player.isHit = false;
        player.attack.enabled = false;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
