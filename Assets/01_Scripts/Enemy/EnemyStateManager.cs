using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    public GameObject player { get; set; }
    public PlayerStateManager playerState { get; private set; }
    public Animator animator { get; set; }
    public NavMeshAgent agent { get; set; }

    public float targetDistance { get; set; }

    public bool isHit { get; set; } = false;
    public bool isReady { get; set; } = false;
    public bool isAttacking { get; set; } = false;
    public bool RotateToPlayer { get; set; } = false;
    public bool rotationLocked { get; set; } = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerState = player.GetComponent<PlayerStateManager>();
        animator = GetComponent<Animator>();
        agent =  GetComponent<NavMeshAgent>();
    }

    public void Activate()
    {
        animator.SetBool("isReady", true);
    }
}