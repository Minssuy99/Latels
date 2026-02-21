using UnityEngine;

public class PlayerStateManager : MonoBehaviour, IBattleComponent
{
    public IState currentState { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerSkillState skillState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerSprintState sprintState { get; private set; }

    public PlayerDash dash { get; private set; }
    public PlayerAttack attack  { get; private set; }
    public PlayerMovement move { get; private set; }

    public bool isHit { get; set; }
    public bool isLockedOn { get; set; }
    public bool isAttacking { get; set; }
    public bool isInvincible { get; set; }
    public bool IsDead => currentState is PlayerDeadState;
    public bool IsDashing => currentState is PlayerDashState;
    public bool IsUsingSkill => currentState is PlayerSkillState;
    public bool IsSprinting => currentState is PlayerSprintState;

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
        sprintState = new PlayerSprintState(this);

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
        float speed = 10f * TimeManager.Instance.PlayerDelta;

        if (isAttacking)
        {
            bool isMoving = move.moveDirection.sqrMagnitude > 0.1f;

            if (isMoving)
            {
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0.0f, speed));
                animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 1.0f, speed));
            }
            else
            {
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1.0f, speed));
                animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0.0f, speed));
            }
        }
        else
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0.0f, speed));
            animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0.0f, speed));
        }
    }
}