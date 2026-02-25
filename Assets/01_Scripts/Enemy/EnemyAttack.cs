using UnityEngine;

public class EnemyAttack : MonoBehaviour, IDamageable
{
    public float HP { get; private set; }
    public float MaxHP => enemy.Data.stats.health;

    private EnemyStateManager enemy;
    [HideInInspector] public CapsuleCollider capsuleCollider;

    [HideInInspector] public bool superArmor;
    [HideInInspector] public int attackType;
    [HideInInspector] public float attackCooldown;

    [SerializeField] private float hitCooldownDuration = 0.075f;
    [HideInInspector] public int hitCount;
    private float hitCooldown;

    [Header("히트박스")]
    [SerializeField] private GameObject[] PunchHitboxes;
    [SerializeField] private GameObject[] KickHitboxes;
    [Header("Danger Zone")]
    [SerializeField] private GameObject[] PunchDangerZones;
    [SerializeField] private GameObject[] KickDangerZones;

    private EnemyHitEffect hitEffect;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemy = GetComponent<EnemyStateManager>();
        hitEffect = GetComponent<EnemyHitEffect>();
    }

    private void Start()
    {
        attackCooldown = enemy.Data.stats.attackCooldown;
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
        if (!other.CompareTag("PlayerHitbox")) return;

        CharacterSetup setup = other.gameObject.GetComponentInParent<CharacterSetup>();
        TakeDamage(setup.Data.stats.damage);
    }

    private void InterruptAttack()
    {
        enemy.ChangeState(enemy.hitState);
    }

    public void DisableAllHitboxes()
    {
        SetColliders(PunchHitboxes, false);
        SetColliders(KickHitboxes, false);
        SetColliders(PunchDangerZones, false);
        SetColliders(KickDangerZones, false);
    }

    public void SetHitbox(int action)
    {
        switch (action)
        {
            case 0: SetColliders(PunchHitboxes, true); break;
            case 1: SetColliders(PunchHitboxes, false); break;
            case 2: SetColliders(KickHitboxes, true); break;
            case 3: SetColliders(KickHitboxes, false); break;
        }
    }

    public void SetDangerZone(int action)
    {
        switch (action)
        {
            case 0: SetColliders(PunchDangerZones, true); break;
            case 1: SetColliders(PunchDangerZones, false); break;
            case 2: SetColliders(KickDangerZones, true); break;
            case 3: SetColliders(KickDangerZones, false); break;
        }
    }

    public void DisableCollider()
    {
        capsuleCollider.enabled = false;
    }

    private void SetColliders(GameObject[] objects, bool active)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(active);
        }
    }

    public void TakeDamage(float damage)
    {
        if (hitCooldown > 0) return;

        hitEffect.HitFlash();
        HP -= damage;
        hitCooldown = hitCooldownDuration;

        if (HP <= 0)
        {
            enemy.ChangeState(enemy.deadState);
            return;
        }

        if (Time.timeScale < 1f)
        {
            DisableAllHitboxes();
            return;
        }

        if (enemy.playerState.IsUsingSkill)
        {
            DisableAllHitboxes();
            return;
        }

        if (superArmor)
        {
            return;
        }

        if (enemy.currentState is EnemyAttackState)
        {
            if (hitCount >= enemy.Data.stats.superArmorCount)
            {
                hitCount = 0;
                superArmor = true;
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
}
