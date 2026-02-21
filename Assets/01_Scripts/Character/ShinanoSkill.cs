using System.Collections;
using UnityEngine;

public class ShinanoSkill : PlayerSkill, ISkillComponent
{
    private EnemyStateManager enemyState;

    public void OnMainSkillEnd()
    {
        enemyState.rotationLocked = false;
        if (playerState.move.moveDirection.sqrMagnitude > 0f)
            playerState.ChangeState(playerState.moveState);
        else
            playerState.ChangeState(playerState.idleState);
        StartCoroutine(InvincibleAfterSkill());
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
        for (int i = 0; i < 6; i++)
        {
            if (enemyState == null) break;

            enemyState.attack.TakeDamage(5);

            if (enemyState.attack.HP <= 0) break;

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

    IEnumerator InvincibleAfterSkill()
    {
        playerState.isInvincible = true;
        yield return new WaitForSeconds(1f);
        playerState.isInvincible = false;
    }
}
