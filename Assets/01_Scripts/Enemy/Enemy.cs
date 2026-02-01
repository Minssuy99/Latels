using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float HP = 100f;

    public event Action OnDeath;

    private GameObject player;
    private CapsuleCollider _collider;
    private Animator animator;
    private Vector3 startPos;
    private Quaternion startRot;
    private bool isHit = false;
    private bool RotateToPlayer = false;

    private float distance;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();

        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (!player)
            return;

        if (RotateToPlayer)
        {
            transform.LookAt(player.transform);
            RotateToPlayer = false;
        }

        if (isHit)
        {
            return;
        }

        distance = Vector3.Distance(transform.position, player.transform.position);
        transform.LookAt(player.transform);

        if (distance <= attackRange)
        {
            animator.SetBool("isRunning", false);

        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,
                player.transform.position, moveSpeed * Time.deltaTime);
            animator.SetBool("isRunning", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            HP -= 20;
            animator.SetTrigger("Hit");
            isHit = true;
            RotateToPlayer = true;

            if (HP <= 0)
            {
                OnDeath?.Invoke();
                animator.SetTrigger("Die");
                _collider.enabled = false;
                enabled = false;
                StartCoroutine(SinkAndDestroy());
            }
            else
            {
                Debug.Log($"HP : " + HP);
            }
        }
    }

    public void OnHitEnd()
    {
        isHit = false;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
