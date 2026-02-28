public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        if (player.move.moveDirection.sqrMagnitude <= 0f)
        {
            player.ChangeState(player.idleState);
            return;
        }

        player.move.HandleMovement();
        player.move.HandleRotation();
        player.move.ApplyRotation();
        player.move.UpdateAnimParameter();
        player.attack.UpdateAttackLayers();
    }

    public override void Exit()
    {

    }
}