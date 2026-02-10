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
        if (playerState.IsUsingSkill) return;
        if (playerState.IsDead) return;
        if (playerState.targetEnemy == null) return;

        playerState.ChangeState(playerState.skillState);
    }

    public void OnMainSkillEnd()
    {
        enemyState.rotationLocked = false;
        playerState.ChangeState(playerState.idleState);
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
}