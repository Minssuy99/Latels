using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private EnemyStateManager enemyState;

    private Vector3 startPos;
    private Quaternion startRot;

    private void Awake()
    {
        enemyState = GetComponent<EnemyStateManager>();
    }

    private void Start()
    {
        enemyState.agent.updateRotation = false;

        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (!enemyState.player)
            return;

        if (enemyState.playerState.isDead)
        {
            enemyState.animator.SetBool("isRunning", false);
            enemyState.animator.SetBool("isReady", false);
            enemyState.agent.isStopped = true;
            transform.LookAt(enemyState.player.transform);
            return;
        }

        if (enemyState.RotateToPlayer)
        {
            transform.LookAt(enemyState.player.transform);
            enemyState.RotateToPlayer = false;
        }

        if (enemyState.isHit)
        {
            return;
        }

        if (!enemyState.isReady)
            return;

        if (enemyState.isAttacking)
        {
            if (!enemyState.rotationLocked)
            {
                transform.LookAt(enemyState.player.transform);
            }
            return;
        }

        enemyState.targetDistance = Vector3.Distance(transform.position, enemyState.player.transform.position);
        transform.LookAt(enemyState.player.transform);

        if (enemyState.targetDistance <= enemyState.agent.stoppingDistance)
        {
            enemyState.agent.isStopped = true;
            enemyState.animator.SetBool("isRunning", false);

        }
        else
        {
            enemyState.agent.isStopped = false;
            enemyState.agent.SetDestination(enemyState.player.transform.position);
            enemyState.animator.SetBool("isRunning", true);
        }
    }

    public void Ready()
    {
        enemyState.isReady = true;
    }
}
