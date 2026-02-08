using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{
    private PlayerStateManager playerState;
    private EnemyStateManager enemyState;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    public void OnMainSkill(InputValue value)
    {
        if (playerState.isUsingMainSkill) return;
        if (playerState.isDead) return;
        if (playerState.targetEnemy == null) return;

        playerState.isAttacking = false;
        playerState.isHit = false;
        playerState.animator.SetLayerWeight(1, 0f);
        playerState.animator.SetLayerWeight(2, 0f);
        playerState.animator.SetLayerWeight(3, 0f);
        playerState.animator.SetLayerWeight(4, 0f);
        playerState.animator.ResetTrigger("Attack");

        if (playerState.isDashing)
        {
            playerState.isDashing = false;
        }

        Vector3 direction = playerState.targetEnemy.transform.position - transform.position;
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);
        playerState.isUsingMainSkill = true;
        playerState.animator.SetTrigger("MainSkill");
    }

    public void OnMainSkillEnd()
    {
        enemyState.rotationLocked = false;
        playerState.isUsingMainSkill = false;
        playerState.animator.applyRootMotion = true;
    }

    public void OnTeleportBehindEnemy()
    {
        enemyState = playerState.targetEnemy.GetComponent<EnemyStateManager>();

        playerState.animator.applyRootMotion = false;
        playerState.characterController.enabled = false;
        enemyState.rotationLocked = true;

        Vector3 direction = (playerState.targetEnemy.transform.position - transform.position).normalized;

        Vector3 pos = playerState.targetEnemy.transform.position + direction * 0.2f;
        pos += playerState.targetEnemy.transform.right * 0.1f;

        transform.position = pos;

        transform.LookAt(playerState.targetEnemy.transform.position);

        playerState.characterController.enabled = true;

        StartCoroutine(MultiHitCoroutine());
    }

    IEnumerator MultiHitCoroutine()
    {
        EnemyAttack ea = enemyState.GetComponent<EnemyAttack>();

        for (int i = 0; i < 6; i++)
        {
            ea.HP -= 5;
            if (ea.HP <= 0)
            {
                ea.HitFlash();
                enemyState.ChangeState(enemyState.deadState);
                break;
            }
            ea.HitFlash();
            if (i < 5)
            {
                yield return new WaitForSeconds(0.2f);
            }
        }

        if (enemyState != null && enemyState.enabled)
        {
            enemyState.animator.SetTrigger("Hit");
        }
    }
}
