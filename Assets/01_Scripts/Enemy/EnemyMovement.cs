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

        if (!enemyState.agent.enabled) return;

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
            if (Time.timeScale > 0.1)
            {
                transform.LookAt(enemyState.player.transform);
            }
            enemyState.RotateToPlayer = false;
        }
    }

    public void Ready()
    {
        enemyState.ChangeState(enemyState.chaseState);
    }
}
