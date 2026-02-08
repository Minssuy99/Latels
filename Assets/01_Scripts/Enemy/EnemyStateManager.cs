using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Inactive,
    Ready,
    Chasing,
    Attacking,
    Hit,
    Dead
}

public class EnemyStateManager : MonoBehaviour
{
    public IState currentState { get; private set; } = null;

    public EnemyAttack attack { get; private set; }

    public EnemyInactiveState inactiveState { get; private set; }
    public EnemyChaseState chaseState { get; private set; }
    public EnemyAttackState attackState { get; private set; }
    public EnemyHitState hitState { get; private set; }
    public EnemyReadyState readyState { get; private set; }
    public EnemyDeadState deadState { get; private set; }



    public GameObject player { get; set; }
    public PlayerStateManager playerState { get; private set; }
    public Animator animator { get; set; }
    public NavMeshAgent agent { get; set; }

    public float targetDistance { get; set; }

    public bool RotateToPlayer { get; set; } = false;
    public bool rotationLocked { get; set; } = false;

    public Area area { get; set; }

    private void Awake()
    {
        inactiveState = new EnemyInactiveState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        hitState = new EnemyHitState(this);
        readyState = new EnemyReadyState(this);
        deadState = new EnemyDeadState(this);

        player = GameObject.FindGameObjectWithTag("Player");
        attack = GetComponent<EnemyAttack>();
        playerState = player.GetComponent<PlayerStateManager>();
        animator = GetComponent<Animator>();
        agent =  GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        ChangeState(inactiveState);
    }

    private void Update()
    {
        currentState.Update();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    public void Activate(Area area)
    {
        ChangeState(readyState);
        this.area = area;
    }

    public IEnumerator SinkAndDestroy()
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


}