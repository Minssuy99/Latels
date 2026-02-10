using UnityEngine;
using System.Collections.Generic;


public class PlayerAttack : MonoBehaviour, IDamageable
{
    [SerializeField] private float HP = 100;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float lockOnRange = 3.0f;
    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();

    private PlayerStateManager playerState;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        if (playerState.IsDashing) return;
        if (!playerState.canAttack) return;
        if (playerState.IsUsingSkill) return;

        playerState.targetDistance = SelectNearestEnemy();

        UpdateLockOn();
        UpdateAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerState.IsDead) return;

        if (other.CompareTag("Area"))
        {
            HandleAreaEnter(other);
        }

        if (other.CompareTag("EnemyHitbox"))
        {
            TakeDamage(5);
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
            hitbox.SetActive(false);
        }
        return targetDistance;
    }

    private void UpdateLockOn()
    {
        if (playerState.targetEnemy != null)
        {
            if (playerState.targetDistance <= lockOnRange)
            {
                playerState.isLockedOn = true;
                playerState.animator.SetBool("isLockedOn", true);
            }
            else
            {
                if (playerState.isLockedOn)
                {
                    playerState.animator.SetBool("isLockedOn", false);
                    playerState.isLockedOn = false;
                }
            }
        }
        else
        {
            playerState.isLockedOn = false;
            playerState.animator.SetBool("isLockedOn", false);
        }
    }

    private void UpdateAttack()
    {
        if (!playerState.targetEnemy)
        {
            playerState.animator.ResetTrigger("Attack");
            return;
        }

        if (playerState.targetDistance <= attackRange)
        {
            playerState.isAttacking = true;
            playerState.animator.SetTrigger("Attack");
        }
        else
        {
            playerState.isAttacking = false;
            hitbox.SetActive(false);
        }
    }

    private void HandleAreaEnter(Collider other)
    {
        Area area = other.GetComponent<Area>();
        enemies = area.GetEnemies();
    }

    public void EnableHitbox()
    {
        hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitbox.SetActive(false);
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

    public void TakeDamage(float damage)
    {
        if (playerState.IsDashing) return;

        HP -= damage;

        if (HP <= 0)
        {
            playerState.ChangeState(playerState.deadState);
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
