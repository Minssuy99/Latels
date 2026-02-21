using UnityEngine;
using System.Collections;

public class ShinanoSkill : PlayerSkill
{
    private EnemyStateManager enemyState;

    public void OnMainSkillEnd()
    {
        enemyState.rotationLocked = false;
        if (player.move.moveDirection.sqrMagnitude > 0f)
            player.ChangeState(player.moveState);
        else
            player.ChangeState(player.idleState);
        StartCoroutine(InvincibleAfterSkill());
    }

    public void OnTeleportBehindEnemy()
    {
        enemyState = player.targetEnemy.GetComponent<EnemyStateManager>();

        player.animator.applyRootMotion = false;
        player.characterController.enabled = false;
        enemyState.rotationLocked = true;

        Vector3 direction = (player.targetEnemy.transform.position - transform.position).normalized;

        Vector3 pos = player.targetEnemy.transform.position + direction * 0.2f;
        pos += player.targetEnemy.transform.right * 0.1f;

        transform.position = pos;

        transform.LookAt(player.targetEnemy.transform.position);

        player.characterController.enabled = true;

        StartCoroutine(MultiHitCoroutine());
    }

    IEnumerator MultiHitCoroutine()
    {
        for (int i = 0; i < 6; i++)
        {
            if (enemyState == null) break;

            enemyState.attack.TakeDamage(player.CharacterData.stats.skillDamage);

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
        player.isInvincible = true;
        yield return new WaitForSeconds(1f);
        player.isInvincible = false;
    }
}
