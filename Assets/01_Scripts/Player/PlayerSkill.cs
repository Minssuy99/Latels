using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{
    public float skillCoolTime = 10f;
    public float remainTime;

    private bool canUseSkill = true;

    private PlayerStateManager playerState;
    private EnemyStateManager enemyState;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        if (canUseSkill == false)
        {
            remainTime -= Time.unscaledDeltaTime;

            if (remainTime <= 0)
                canUseSkill = true;
        }
    }

    public void OnMainSkill(InputValue value)
    {
        if (playerState.IsUsingSkill) return;
        if (playerState.IsDead) return;
        if (playerState.targetEnemy == null) return;
        if (canUseSkill == false) return;

        canUseSkill = false;
        remainTime = skillCoolTime;
        playerState.ChangeState(playerState.skillState);
    }

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