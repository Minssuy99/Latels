using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IBattleComponent
{
    public float HP { get; private set; }
    public float MaxHP => player.CharacterData.stats.health;

    public event Action<float, Vector3> OnDamaged;

    private float hitCooldown;
    private PlayerStateManager player;

    private void Awake()
    {
        player = GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        HP = player.CharacterData.stats.health;
    }

    private void Update()
    {
        if (hitCooldown > 0)
        {
            hitCooldown -= TimeManager.Instance.PlayerDelta;
            if (hitCooldown <= 0)
                player.SetIsHit(false);
        }
    }

    public void TakeDamage(float damage, Vector3 attackerPos)
    {
        if (player.IsDashing) return;
        if (player.IsUsingSkill) return;
        if (player.isInvincible) return;

        if (hitCooldown > 0) return;
        player.SetIsHit(true);
        hitCooldown = 0.1f;

        HP -= damage;
        OnDamaged?.Invoke(damage, attackerPos);

        if (HP <= 0)
        {
            player.ChangeState(player.deadState);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player.IsDead) return;

        if (other.CompareTag(GameTags.EnemyHitbox))
        {
            EnemyStateManager enemy = other.GetComponentInParent<EnemyStateManager>();
            TakeDamage(enemy.Data.stats.damage, enemy.transform.position);
        }
    }
}