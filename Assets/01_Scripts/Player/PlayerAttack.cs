using System;
using UnityEngine;
using System.Collections.Generic;


public class PlayerAttack : MonoBehaviour, IDamageable
{
    [SerializeField] private float hp = 100f;
    private float maxHP;
    public float HP => hp;
    public float MaxHP => maxHP;

    [SerializeField] private GameObject hitbox;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float lockOnRange = 3.0f;

    private float hitCooldown = 0f;

    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();
    public List<EnemyStateManager> GetEnemies() => enemies;

    private PlayerStateManager playerState;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        maxHP = hp;
    }

    private void Update()
    {
        if (hitCooldown > 0)
        {
            hitCooldown -= Time.unscaledDeltaTime;
            if (hitCooldown <= 0)
                playerState.isHit = false;
        }
        if (playerState.IsSprinting) return;
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
            playerState.isAttacking = false;
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

    public void SetEnemies(List<EnemyStateManager> enemies)
    {
        this.enemies = enemies;
    }

    public void EnableHitbox()
    {
        hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitbox.SetActive(false);
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
        if (playerState.IsUsingSkill) return;
        if (playerState.isInvincible) return;

        if (hitCooldown > 0) return;
        playerState.isHit = true;
        hitCooldown = 0.1f;

        hp -= damage;
        InGameUIManager.Instance.ShowDamageEffect();

        if (hp <= 0)
        {
            playerState.ChangeState(playerState.deadState);
        }
    }
}
