using UnityEngine;

public class EnemyReadyState : EnemyBaseState
{
    public EnemyReadyState(EnemyStateManager enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        enemy.animator.SetBool("isReady", true);
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}