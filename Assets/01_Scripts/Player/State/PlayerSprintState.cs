using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.SetIsLockedOn(false);
        player.SetIsAttacking(false);
        player.animator.SetBool(AnimHash.IsLockedOn, false);
    }

    public override void Update()
    {
        if (player.move.MoveDirection.sqrMagnitude <= 0f)
        {
            player.ChangeState(player.idleState);
            return;
        }

        player.move.HandleMovement(true);
        player.move.HandleRotation();
        player.move.ApplyRotation();
        player.move.UpdateAnimParameter();
        player.animator.SetLayerWeight(AnimHash.SprintLayer, Mathf.Lerp(player.animator.GetLayerWeight(AnimHash.SprintLayer), 1f, 10f * TimeManager.Instance.PlayerDelta));
    }

    public override void Exit()
    {
        player.animator.SetLayerWeight(AnimHash.SprintLayer, 0f);
    }
}
