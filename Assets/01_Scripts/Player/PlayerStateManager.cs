using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public IState currentState { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerSkillState skillState { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    public PlayerDash dash { get; private set; }
    public PlayerAttack attack  { get; private set; }
    public PlayerMovement move { get; private set; }

    public bool isHit { get; set; }
    public bool isLockedOn { get; set; }
    public bool isAttacking { get; set; }
    public bool IsDead => currentState is PlayerDeadState;
    public bool IsDashing => currentState is PlayerDashState;
    public bool IsUsingSkill => currentState is PlayerSkillState;

    public bool canAttack { get; set; } = true;

    public GameObject targetEnemy { get; set; }
    public float targetDistance { get; set; }

    public Animator animator { get; private set; }
    public CharacterController characterController { get; private set; }

    private void Awake()
    {
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        dashState = new PlayerDashState(this);
        skillState = new PlayerSkillState(this);
        deadState = new PlayerDeadState(this);

        move = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        dash = GetComponent<PlayerDash>();

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        ChangeState(idleState);
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

    public void UpdateAttackLayers()
    {
        if (isAttacking)
        {
            bool isMoving = move.moveDirection.sqrMagnitude > 0.1f;

            if (isMoving)
            {
                animator.SetLayerWeight(1, 0.0f);
                animator.SetLayerWeight(2, 1.0f);
            }
            else
            {
                animator.SetLayerWeight(1, 1.0f);
                animator.SetLayerWeight(2, 0.0f);
            }
        }
        else
        {
            animator.SetLayerWeight(1, 0.0f);
            animator.SetLayerWeight(2, 0.0f);
        }
    }

    public void UpdateHitLayers()
    {
        if (isHit)
        {
            bool isMoving = move.moveDirection.sqrMagnitude > 0.1f;

            if (isMoving || isAttacking)
            {
                animator.SetLayerWeight(3, 0f);
                animator.SetLayerWeight(4, 1f);
            }
            else
            {
                animator.SetLayerWeight(3, 1f);
                animator.SetLayerWeight(4, 0f);
            }
        }
        else
        {
            animator.SetLayerWeight(3, 0f);
            animator.SetLayerWeight(4, 0f);
        }
    }
}