using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttack : MonoBehaviour, IDamageable
{
    [SerializeField] private float hp = 100f;
    private float maxHP;
    public float HP => hp;
    public float MaxHP => maxHP;

    private EnemyStateManager enemyState;
    [HideInInspector] public CapsuleCollider capsuleCollider;

    // 전투 설정
    [HideInInspector] public bool superArmor = false;
    [HideInInspector] public int attackType;
    [HideInInspector] public float attackCooldown;

    // 피격
    [SerializeField] private float hitCooldownDuration = 0.1f;
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
        enemyState = GetComponent<EnemyStateManager>();
        hitEffect = GetComponent<EnemyHitEffect>();
    }

    private void Start()
    {
        attackCooldown = Random.Range(1, 3);
        maxHP = hp;
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

        TakeDamage(5);
    }

    private void InterruptAttack()
    {
        enemyState.ChangeState(enemyState.hitState);
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
        hp -= damage;
        hitCooldown = hitCooldownDuration;

        if (hp <= 0)
        {
            enemyState.ChangeState(enemyState.deadState);
            return;
        }

        if (Time.timeScale < 1f)
        {
            DisableAllHitboxes();
            return;
        }

        if (enemyState.playerState.IsUsingSkill)
        {
            DisableAllHitboxes();
            return;
        }

        if (superArmor)
        {
            return;
        }

        if (enemyState.currentState is EnemyAttackState)
        {
            if (hitCount >= 3)
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
