using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour, IDamageable
{
    private float hitCooldown = 0f;

    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();
    public List<EnemyStateManager> GetEnemies() => enemies;
    public float HP { get; private set; }
    public float MaxHP => playerData.stats.health;

    private PlayerStateManager playerState;
    private CharacterData playerData;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        playerData = GameManager.Instance.characterSlots[0];
        HP = playerData.stats.health;
    }

    private void Update()
    {
        if (hitCooldown > 0)
        {
            hitCooldown -= TimeManager.Instance.PlayerDelta;
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
            DisableHitbox();
        }
        return targetDistance;
    }

    private void UpdateLockOn()
    {
        if (playerState.targetEnemy != null)
        {
            if (playerState.targetDistance <= playerData.stats.lockOnRange)
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

        if (playerState.targetDistance <= playerData.stats.attackRange)
        {
            playerState.isAttacking = true;
            ExecuteAttack();
        }
        else
        {
            playerState.isAttacking = false;
            DisableHitbox();
        }
    }

    public void SetEnemies(List<EnemyStateManager> enemies)
    {
        this.enemies = enemies;
    }

    public virtual void EnableHitbox()
    {

    }

    public virtual void DisableHitbox()
    {

    }

    private void OnDrawGizmosSelected()
    {
        if (playerData == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, playerData.stats.lockOnRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerData.stats.attackRange);

        if (playerState == null || playerState.targetEnemy == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, playerState.targetEnemy.transform.position);
    }

    public virtual void ExecuteAttack()
    {
        playerState.animator.SetTrigger("Attack");
    }

    public void TakeDamage(float damage)
    {
        if (playerState.IsDashing) return;
        if (playerState.IsUsingSkill) return;
        if (playerState.isInvincible) return;

        if (hitCooldown > 0) return;
        playerState.isHit = true;
        hitCooldown = 0.1f;

        HP -= damage;
        InGameUIManager.Instance.ShowDamageEffect();

        if (HP <= 0)
        {
            playerState.ChangeState(playerState.deadState);
        }
    }
}
