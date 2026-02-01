using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject Hitbox;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float lockOnRange = 3.0f;
    List<Enemy> enemies = new List<Enemy>();

    private PlayerStateManager playerState;

    private void Start()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Area"))
        {
            Area area = other.GetComponent<Area>();
            enemies = area.GetEnemies();
        }
    }

    private void Update()
    {
        playerState.targetDistance = SelectNearestEnemy();

        if (playerState.targetEnemy != null)
        {
            if (playerState.targetDistance <= lockOnRange)
            {
                playerState.isLockedOn = true;
                playerState.animator.SetBool("isLockedOn", true);
                Debug.LogWarning($"Locking on! : " + playerState.targetEnemy.name);
            }
            else
            {
                if (playerState.isLockedOn)
                {
                    playerState.animator.SetBool("isLockedOn", false);
                    playerState.isLockedOn = false;
                    Debug.LogWarning("Locking off");
                }
            }
        }
        else
        {
            playerState.isLockedOn = false;
            playerState.animator.SetBool("isLockedOn", false);
        }

        if (!playerState.targetEnemy)
        {
            playerState.animator.ResetTrigger("Attack");
            return;
        }

        if (playerState.targetDistance <= attackRange)
        {
            Debug.LogWarning($"Attack! : " + playerState.targetEnemy.name);
            playerState.isAttacking = true;
            playerState.animator.SetTrigger("Attack");
        }
        else
        {
            playerState.isAttacking = false;
        }
    }

    private float SelectNearestEnemy()
    {
        if (playerState == null) return Mathf.Infinity;

        float targetDistance = Mathf.Infinity;
        playerState.targetEnemy = null;
        if (enemies.Count > 0)
        {
            foreach (Enemy enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < targetDistance)
                {
                    targetDistance = distance;
                    playerState.targetEnemy = enemy.gameObject;
                }
            }
            if(playerState.targetEnemy != null)
                Debug.Log(playerState.targetEnemy.name);
        }
        else
        {
            playerState.targetEnemy = null;
            Hitbox.SetActive(false);
        }
        return targetDistance;
    }

    public void EnableHitbox()
    {
        Hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        Hitbox.SetActive(false);
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
}
