using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.isLockedOn = false;
        player.isAttacking = false;
        player.animator.SetBool("isLockedOn", false);
    }

    public override void Update()
    {
        if (player.move.moveDirection.sqrMagnitude <= 0f)
        {
            player.ChangeState(player.idleState);
            return;
        }

        player.move.HandleMovement(true);
        player.move.HandleRotation();
        player.move.ApplyRotation();
        player.move.UpdateAnimParameter();
        player.animator.SetLayerWeight(3, Mathf.Lerp(player.animator.GetLayerWeight(3), 1f, 10f * TimeManager.Instance.PlayerDelta));
    }

    public override void Exit()
    {
        player.animator.SetLayerWeight(3, 0f);
    }
}
