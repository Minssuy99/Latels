using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float hitCooldownDuration = 0.075f;
    public float HP { get; private set; }
    public float MaxHP => enemy.Health;
    public int HitCount { get; set; }
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

    public void ResetHitCount()
    {
        HitCount = 0;
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
            InGameUIManager.Instance.UnsubscribeEnemy(this);
            return;
        }

        if (TimeManager.Instance.IsSlowMotion)
        {
            enemy.attack.DisableAllHitboxes();
            return;
        }

        if (enemy.attack.SuperArmor)
        {
            return;
        }

        if (enemy.currentState is EnemyAttackState)
        {
            if (HitCount >= enemy.SuperArmorCount)
            {
                HitCount = 0;
                enemy.attack.ActivateSuperArmor();
            }
            else
            {
                InterruptAttack();
                HitCount++;
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
