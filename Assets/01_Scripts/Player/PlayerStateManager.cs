using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public bool isAttacking { get; set; } = false;
    public bool isLockedOn { get; set; } = false;
    public bool isHit { get; set; } = false;
    public bool isDead { get; set; } = false;
    public bool isDashing { get; set; } = false;

    public bool canAttack { get; set; } = true;

    public GameObject targetEnemy { get; set; }
    public float targetDistance { get; set; }

    public Animator animator { get; set; }
    public CharacterController characterController { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
}
