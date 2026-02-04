using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float HP = 100f;
    [SerializeField] private bool superArmor = false;
    [SerializeField] private GameObject[] PunchHitboxes;
    [SerializeField] private GameObject[] KickHitboxes;
    [SerializeField] private GameObject[] PunchDangerZones;
    [SerializeField] private GameObject[] KickDangerZones;

    private EnemyStateManager enemyState;
    private CapsuleCollider _collider;

    private int attackType;
    private float attackRange;
    private float attackCooldown;
    private float hitCooldown = 0f;
    private float hitCooldownDuration = 0.1f;

    /****************************************/
    /************* Unity Events *************/
    /****************************************/

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        enemyState = GetComponent<EnemyStateManager>();

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

        if (Time.timeScale < 1f && enemyState.isAttacking)
        {
            enemyState.rotationLocked = true;
        }

        if (enemyState.playerState.isDead) return;

        if (!enemyState.isReady) return;

        if (!enemyState.isAttacking && enemyState.targetDistance <= attackRange)
        {
            attackCooldown -= Time.deltaTime;

            if (attackCooldown <= 0)
            {
                attackType =  Random.Range(0, 3);
                enemyState.animator.SetInteger("AttackType", attackType);
                enemyState.animator.SetTrigger("Attack");
                enemyState.isAttacking = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            if (hitCooldown > 0) return;

            hitCooldown = hitCooldownDuration;

            HP -= 5;
            Debug.Log(HP);

            if (HP <= 0)
            {
                enemyState.animator.SetTrigger("Die");
                _collider.enabled = false;
                enabled = false;
                enemyState.agent.enabled = false;
                StartCoroutine(SinkAndDestroy());
            }
            else if ((enemyState.isAttacking && Random.value <= 0.5) || superArmor)
            {
                enemyState.isAttacking = false;
            }
            else
            {
                enemyState.animator.Play("Base Layer.Hit", 0, 0f);
                enemyState.isHit = true;
                enemyState.RotateToPlayer = true;
                enemyState.isAttacking = false;
                attackCooldown = Random.Range(1, 3);

                foreach (var hitbox in PunchHitboxes)
                {
                    hitbox.SetActive(false);
                }

                foreach (var hitbox in KickHitboxes)
                {
                    hitbox.SetActive(false);
                }
            }
        }
    }

    IEnumerator SinkAndDestroy()
    {
        float sinkSpeed = 0.25f;
        float sinkDistance = 1f;
        float sunk = 0f;

        yield return new WaitForSeconds(3f);
        while (sunk < sinkDistance)
        {
            float delta = sinkSpeed * Time.deltaTime;
            transform.position -= Vector3.up * delta;
            sunk += delta;
            yield return null;
        }
        Destroy(gameObject);
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
        enemyState.isAttacking = false;
        attackCooldown = Random.Range(1, 3);
        enemyState.rotationLocked = false;
    }

    public void OnHitEnd()
    {
        enemyState.isHit = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
