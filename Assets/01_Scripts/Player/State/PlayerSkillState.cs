using UnityEngine;

public class PlayerSkillState : PlayerBaseState
{
    public PlayerSkillState(PlayerStateManager player) : base(player)
    {
    }

    public override void Enter()
    {
        player.isAttacking = false;
        player.isHit = false;
        player.animator.SetLayerWeight(1, 0f);
        player.animator.SetLayerWeight(2, 0f);
        player.animator.SetLayerWeight(3, 0f);
        player.animator.SetLayerWeight(4, 0f);
        player.animator.ResetTrigger("Attack");

        Vector3 direction = player.targetEnemy.transform.position - player.transform.position;
        direction.y = 0;

        player.transform.rotation = Quaternion.LookRotation(direction);
        player.animator.SetTrigger("MainSkill");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        player.animator.applyRootMotion = true;
    }
}
