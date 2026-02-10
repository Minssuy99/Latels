using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        if (player.move.moveDirection.sqrMagnitude > 0f)
        {
            player.ChangeState(player.moveState);
            return;
        }

        player.move.HandleRotation();
        player.move.ApplyRotation();
        player.move.UpdateAnimParameter();

        player.UpdateAttackLayers();
        player.UpdateHitLayers();
    }

    public override void Exit()
    {

    }
}