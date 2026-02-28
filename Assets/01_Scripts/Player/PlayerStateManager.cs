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

    public CharacterSetup setup { get; private set; }
    public CharacterData CharacterData => setup.Data;
    public PlayerMovement move { get; private set; }
    public PlayerDash dash { get; private set; }
    public PlayerAttack attack  { get; private set; }
    public PlayerHealth health  { get; private set; }
    public PlayerMainSkill mainSkill { get;  private set; }
    public SupportSkill supportSkill { get;  private set; }
    public TargetDetector targetDetector { get; private set; }
    public LockOnController lockOnController { get; private set; }

    public bool isHit { get; private set; }
    public bool isLockedOn { get; private set; }
    public bool isAttacking { get; private set; }
    public bool isAttackFinishing {get; private set;}
    public bool isInvincible { get; private set; }
    public bool canAttack { get; private set; } = true;
    public bool IsDead => currentState is PlayerDeadState;
    public bool IsDashing => currentState is PlayerDashState;
    public bool IsUsingSkill => currentState is PlayerSkillState;
    public bool IsSprinting => currentState is PlayerSprintState;

    public EnemyStateManager targetEnemy { get; set; }
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

        setup = GetComponent<CharacterSetup>();
        move = GetComponent<PlayerMovement>();
        dash = GetComponent<PlayerDash>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
        mainSkill =  GetComponent<PlayerMainSkill>();
        supportSkill = GetComponent<SupportSkill>();
        targetDetector = GetComponent<TargetDetector>();
        lockOnController = GetComponent<LockOnController>();

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

    public void SetIsHit(bool  value) => isHit = value;
    public void SetIsLockedOn(bool value) => isLockedOn = value;
    public void SetIsAttacking(bool value) => isAttacking = value;
    public void SetIsAttackFinishing(bool value) => isAttackFinishing = value;
    public void SetIsInvincible(bool value) => isInvincible = value;
    public void SetCanAttack(bool value) => canAttack = value;

}