using UnityEngine;
using System.Collections;

public class ShinanoMainSkill : PlayerMainSkill
{
    private EnemyStateManager enemy;

    public void OnMainSkillEnd()
    {
        enemy.rotationLocked = false;
        StartCoroutine(InvincibleAfterSkill());
        EndSkill();
    }

    public void OnTeleportBehindEnemy()
    {
        enemy = skillTarget.GetComponent<EnemyStateManager>();

        player.animator.applyRootMotion = false;
        player.characterController.enabled = false;
        enemy.rotationLocked = true;

        Vector3 direction = (skillTarget.transform.position - transform.position).normalized;

        Vector3 pos = skillTarget.transform.position + direction * 0.2f;
        pos += skillTarget.transform.right * 0.1f;

        transform.position = pos;

        transform.LookAt(skillTarget.transform.position);

        player.characterController.enabled = true;

        StartCoroutine(MultiHitCoroutine());
    }

    IEnumerator MultiHitCoroutine()
    {
        for (int i = 0; i < 6; i++)
        {
            if (enemy == null) break;

            enemy.health.TakeDamage(player.CharacterData.stats.skillDamage, transform.position);

            if (enemy.health.HP <= 0) break;

            if (i < 5)
            {
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }

        if (enemy != null && enemy.enabled)
        {
            enemy.animator.SetTrigger(AnimHash.Hit);
        }
    }

    IEnumerator InvincibleAfterSkill()
    {
        player.SetIsInvincible(true);
        yield return new WaitForSecondsRealtime(1f);
        player.SetIsInvincible(false);
    }
}
