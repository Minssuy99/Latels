using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] public float HP = 100f;
    [SerializeField] public bool superArmor = false; // 디버깅용
    [SerializeField] private GameObject[] PunchHitboxes;
    [SerializeField] private GameObject[] KickHitboxes;
    [SerializeField] private GameObject[] PunchDangerZones;
    [SerializeField] private GameObject[] KickDangerZones;

    private EnemyStateManager enemyState;
    public CapsuleCollider capsuleCollider;

    public int attackType;
    public float attackRange;
    public float attackCooldown;
    public float hitCooldown = 0f;
    public float hitCooldownDuration = 0.1f;
    public int hitCount;

    public Renderer[] renderers;
    public Material[][] originalMaterials;
    [SerializeField] private Material HitMaterial;

    /****************************************/
    /************* Unity Events *************/
    /****************************************/

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemyState = GetComponent<EnemyStateManager>();

        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    private void Start()
    {
        attackRange = enemyState.agent.stoppingDistance;
        attackCooldown = Random.Range(1, 3);
    }

    private void Update()
    {
        if (hitCooldown > 0)
        {
            hitCooldown -= Time.unscaledDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerHitbox")) return;

        Debug.Log("I'm Hit");
        HitFlash();
        HP -= 5;
        hitCooldown = hitCooldownDuration;

        if (HP <= 0)
        {
            enemyState.ChangeState(enemyState.deadState);
            return;
        }

        if (Time.timeScale < 1f)
        {
            DiableAllHitboxes();
            return;
        }

        if (enemyState.playerState.isUsingMainSkill)
        {
            DiableAllHitboxes();
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
                Debug.Log($"HitCount: {hitCount}");
            }
            return;
        }
        enemyState.RotateToPlayer = true;
        InterruptAttack();
    }

    private void InterruptAttack()
    {
        enemyState.ChangeState(enemyState.hitState);
    }

    public void DiableAllHitboxes()
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

    public void HitFlash()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            int count = originalMaterials[i].Length;
            Material[] newMats = new Material[count];

            for (int j = 0; j < count; j++)
            {
                newMats[j] = HitMaterial;
            }
            renderers[i].materials = newMats;
        }

        StartCoroutine(RestoreMaterials());
    }

    IEnumerator RestoreMaterials()
    {
        HitMaterial.SetColor("_BaseColor", Color.red);
        yield return new WaitForSecondsRealtime(0.1f);
        HitMaterial.SetColor("_BaseColor", Color.white);
        yield return new WaitForSecondsRealtime(0.1f);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }
    }

    /****************************************/
    /*********** Animation Events ***********/
    /****************************************/

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

    public void OnAttackEnd()
    {
        enemyState.ChangeState(enemyState.chaseState);
    }

    public void OnHitEnd()
    {
        enemyState.ChangeState(enemyState.chaseState);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
