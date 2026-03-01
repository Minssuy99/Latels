public class PlayerSkillState : PlayerBaseState
{
    public PlayerSkillState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.SetCanAttack(true);
        player.SetIsAttacking(false);
        player.animator.SetLayerWeight(1, 0f);
        player.animator.SetLayerWeight(2, 0f);
        player.animator.ResetTrigger(AnimHash.Attack);
        player.mainSkill.OnSkillStart();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        player.animator.applyRootMotion = true;
    }
}
