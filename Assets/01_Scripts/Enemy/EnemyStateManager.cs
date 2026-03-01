using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyStateManager : MonoBehaviour
{
    public IState currentState { get; private set; }

    public EnemyData Data { get; private set; }
    public EnemyAttack attack { get; private set; }
    public EnemyHealth health { get; private set; }

    public EnemyInactiveState inactiveState { get; private set; }
    public EnemyChaseState chaseState { get; private set; }
    public EnemyAttackState attackState { get; private set; }
    public EnemyHitState hitState { get; private set; }
    public EnemyReadyState readyState { get; private set; }
    public EnemyDeadState deadState { get; private set; }

    public GameObject player { get; private set; }
    public Vector3 playerPos => new (player.transform.position.x, transform.position.y, player.transform.position.z);
    public PlayerStateManager playerState { get; private set; }
    public Animator animator { get; private set; }
    public NavMeshAgent agent { get; set; }

    public float targetDistance { get; set; }
    public bool rotationLocked { get; set; }

    public Area area { get; private set; }

    private void Awake()
    {
        inactiveState = new EnemyInactiveState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        hitState = new EnemyHitState(this);
        readyState = new EnemyReadyState(this);
        deadState = new EnemyDeadState(this);

        animator = GetComponentInChildren<Animator>();
        attack = GetComponent<EnemyAttack>();
        health = GetComponent<EnemyHealth>();
        agent =  GetComponent<NavMeshAgent>();
        player =  GameObject.FindGameObjectWithTag(GameTags.Player);
        playerState = player.GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        ChangeState(inactiveState);
        agent.updateRotation = false;
    }

    private void Update()
    {
        if (playerState.IsDead)
        {
            if (currentState is not EnemyInactiveState && currentState is not EnemyDeadState)
            {
                ChangeState(inactiveState);
                return;
            }
        }

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

    public void SetData(EnemyData data)
    {
        Data = data;
        agent.speed = Data.stats.moveSpeed;
        agent.stoppingDistance = Data.stats.attackRange;
    }

    public void SetArea(Area area)
    {
        this.area = area;
    }

    public void Activate()
    {
        ChangeState(readyState);
    }
    public void Ready()
    {
        ChangeState(chaseState);
    }

    public void OnAttackEnd()
    {
        if (currentState is EnemyDeadState) return;
        ChangeState(chaseState);
    }

    public void OnHitEnd()
    {
        if (currentState is EnemyDeadState) return;
        ChangeState(chaseState);
    }

    public IEnumerator SinkAndDestroy()
    {
        float sinkSpeed = 0.25f;
        float sinkDistance = 1f;
        float sunk = 0f;

        float elapsed = 0f;
        while (elapsed < 3f)
        {
            elapsed += TimeManager.Instance.EnemyDelta;
            yield return null;
        }

        while (sunk < sinkDistance)
        {
            float delta = sinkSpeed * TimeManager.Instance.EnemyDelta;
            transform.position -= Vector3.up * delta;
            sunk += delta;
            yield return null;
        }
        Destroy(gameObject);
    }
}