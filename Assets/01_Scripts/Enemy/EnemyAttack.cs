using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttack : MonoBehaviour, IDamageable
{
    public float HP { get; private set; }
    public float MaxHP => enemy.Data.stats.health;

    private EnemyStateManager enemy;
    [HideInInspector] public CapsuleCollider capsuleCollider;

    // 전투 설정
    [HideInInspector] public bool superArmor = false;
    [HideInInspector] public int attackType;
    [HideInInspector] public float attackCooldown;

    // 피격
    [SerializeField] private float hitCooldownDuration = 0.075f;
    [HideInInspector] public int hitCount;
    private float hitCooldown = 0f;

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
        foreach (var hitbox in PunchHitboxes)
        {
            hitbox.SetActive(false);
        }

        foreach (var hitbox in KickHitboxes)
        {
            hitbox.SetActive(false);
        }

        foreach (var dangerZone in PunchDangerZones)
        {
            dangerZone.SetActive(false);
        }

        foreach (var dangerZone in KickDangerZones)
        {
            dangerZone.SetActive(false);
        }
    }

    public void SetHitbox(int action)
    {
        switch (action)
        {
            case 0: // 주먹 켜기
                foreach(var hitbox in PunchHitboxes)
                    hitbox.SetActive(true);
                break;
            case 1: // 주먹 끄기
                foreach(var hitbox in PunchHitboxes)
                    hitbox.SetActive(false);
                break;
            case 2: // 발차기 켜기
                foreach(var hitbox in KickHitboxes)
                    hitbox.SetActive(true);
                break;
            case 3: // 발차기 끄기
                foreach(var hitbox in KickHitboxes)
                    hitbox.SetActive(false);
                break;
        }
    }

    public void SetDangerZone(int action)
    {
        switch (action)
        {
            case 0: // 주먹 켜기
                foreach(var DangerZone in PunchDangerZones)
                    DangerZone.SetActive(true);
                break;
            case 1: // 주먹 끄기
                foreach(var DangerZone in PunchDangerZones)
                    DangerZone.SetActive(false);
                break;
            case 2: // 발차기 켜기
                foreach(var DangerZone in KickDangerZones)
                    DangerZone.SetActive(true);
                break;
            case 3: // 발차기 끄기
                foreach(var DangerZone in KickDangerZones)
                    DangerZone.SetActive(false);
                break;
        }
    }

    public void DisableCollider()
    {
        capsuleCollider.enabled = false;
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
