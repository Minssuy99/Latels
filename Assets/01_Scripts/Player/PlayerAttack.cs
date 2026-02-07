using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject Hitbox;
    [SerializeField] private float HP = 100;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float lockOnRange = 3.0f;
    List<EnemyStateManager> enemies = new List<EnemyStateManager>();

    private PlayerStateManager playerState;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerState.isDead) return;

        if (other.CompareTag("Area"))
        {
            Area area = other.GetComponent<Area>();
            enemies = area.GetEnemies();
        }

        if (other.CompareTag("EnemyHitbox"))
        {
            if (playerState.isDashing) return;

            HP -= 5;

            if (HP <= 0)
            {
                playerState.animator.SetLayerWeight(1, 0f);
                playerState.animator.SetLayerWeight(2, 0f);
                playerState.animator.SetLayerWeight(3, 0f);
                playerState.animator.SetLayerWeight(4, 0f);

                // Debug.LogWarning("I'm Dead");
                Hitbox.SetActive(false);

                playerState.animator.SetTrigger("Die");

                playerState.isDead = true;
                playerState.isAttacking = false;
                playerState.isHit = false;
                enabled = false;

                return;
            }

            bool isMoving = playerState.characterController.velocity.magnitude > 0.1f;
            bool isAttacking = playerState.isAttacking;

            if (isMoving || isAttacking)
            {
                playerState.animator.SetLayerWeight(3, 0f);  // Full Hit
                playerState.animator.SetLayerWeight(4, 1f);  // Upper Hit
            }
            else
            {
                playerState.animator.SetLayerWeight(3, 1f);  // Full Hit
                playerState.animator.SetLayerWeight(4, 0f);  // Upper Hit
            }
            playerState.animator.SetTrigger("Hit");
            playerState.isHit = true;
        }
    }

    private void Update()
    {
        if (playerState.isDashing) return;
        if (!playerState.canAttack) return;

        playerState.targetDistance = SelectNearestEnemy();

        if (playerState.targetEnemy != null)
        {
            if (playerState.targetDistance <= lockOnRange)
            {
                playerState.isLockedOn = true;
                playerState.animator.SetBool("isLockedOn", true);
                // Debug.LogWarning($"Locking on! : " + playerState.targetEnemy.name);
            }
            else
            {
                if (playerState.isLockedOn)
                {
                    playerState.animator.SetBool("isLockedOn", false);
                    playerState.isLockedOn = false;
                    // Debug.LogWarning("Locking off");
                }
            }
        }
        else
        {
            playerState.isLockedOn = false;
            playerState.animator.SetBool("isLockedOn", false);
        }

        if (!playerState.targetEnemy)
        {
            playerState.animator.ResetTrigger("Attack");
            return;
        }

        if (playerState.targetDistance <= attackRange)
        {
            // Debug.LogWarning($"Attack! : " + playerState.targetEnemy.name);
            playerState.isAttacking = true;
            playerState.animator.SetTrigger("Attack");
        }
        else
        {
            playerState.isAttacking = false;
            Hitbox.SetActive(false);
        }
    }

    private float SelectNearestEnemy()
    {
        if (playerState == null) return Mathf.Infinity;

        float targetDistance = Mathf.Infinity;
        playerState.targetEnemy = null;
        if (enemies.Count > 0)
        {
            foreach (EnemyStateManager enemy in enemies)
            {
                if (enemy == null) continue;

                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < targetDistance)
                {
                    targetDistance = distance;
                    playerState.targetEnemy = enemy.gameObject;
                }
            }
        }
        else
        {
            playerState.targetEnemy = null;
            Hitbox.SetActive(false);
        }
        return targetDistance;
    }

    public void EnableHitbox()
    {
        Hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        Hitbox.SetActive(false);
    }

    public void OnHitEnd()
    {
        playerState.isHit = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, lockOnRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (playerState == null || playerState.targetEnemy == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, playerState.targetEnemy.transform.position);
    }
}
