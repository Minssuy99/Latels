using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float hitCooldownDuration = 0.075f;
    public float HP { get; private set; }
    public float MaxHP => enemy.Data.stats.health;
    public int hitCount { get; set; }
    public event Action<float, Vector3> OnDamaged;
    private float hitCooldown;
    private EnemyStateManager enemy;
    private EnemyHitEffect hitEffect;
    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemy = GetComponent<EnemyStateManager>();
        hitEffect = GetComponent<EnemyHitEffect>();
    }

    private void Start()
    {
        HP = MaxHP;
    }

    private void Update()
    {
        if (hitCooldown > 0)
        {
            hitCooldown -= TimeManager.Instance.PlayerDelta;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(GameTags.PlayerHitbox)) return;

        CharacterSetup setup = other.gameObject.GetComponentInParent<CharacterSetup>();
        TakeDamage(setup.Data.stats.damage, setup.transform.position);
    }

    private void InterruptAttack()
    {
        enemy.ChangeState(enemy.hitState);
    }

    public void TakeDamage(float damage, Vector3 attackerPos)
    {
        if (hitCooldown > 0) return;

        hitEffect.PlayHitEffect();
        HP -= damage;
        hitCooldown = hitCooldownDuration;

        OnDamaged?.Invoke(damage, attackerPos);

        if (HP <= 0)
        {
            enemy.ChangeState(enemy.deadState);
            return;
        }

        if (TimeManager.Instance.IsSlowMotion)
        {
            enemy.attack.DisableAllHitboxes();
            return;
        }

        if (enemy.attack.superArmor)
        {
            return;
        }

        if (enemy.currentState is EnemyAttackState)
        {
            if (hitCount >= enemy.Data.stats.superArmorCount)
            {
                hitCount = 0;
                enemy.attack.superArmor = true;
            }
            else
            {
                InterruptAttack();
                hitCount++;
            }
            return;
        }
        InterruptAttack();
    }

    public void DisableCollider()
    {
        capsuleCollider.enabled = false;
    }
}
