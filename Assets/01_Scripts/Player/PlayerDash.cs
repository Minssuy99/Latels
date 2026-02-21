using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour, IBattleComponent
{
    // 참조
    private PlayerStateManager playerState;
    private DodgeDetector dodgeDetector;

    // 대쉬 설정
    [SerializeField] private float dashDuration = 0.4f;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private float reuseCooldown = 0.8f;
    public float dashSpeed = 8f;

    // 대쉬 스택
    private int maxStack = 3;
    private int currentStack = 3;
    private float chargeTimer = 0f;
    private float reuseTimer = 0f;

    // 런타임
    [HideInInspector] public Vector3 dashDirection;

    // UI용 프로퍼티
    public int CurrentStack => currentStack;
    public int MaxStack => maxStack;
    public float ChargeFillAmount => chargeTimer / chargeTime;
    public bool IsReuseDelay => reuseTimer > 0f;
    public float ReuseTimer => reuseTimer;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
        dodgeDetector = GetComponentInChildren<DodgeDetector>();
    }

    private void Update()
    {
        if (reuseTimer > 0f)
        {
            reuseTimer -= TimeManager.Instance.PlayerDelta;
        }

        if (currentStack < maxStack)
        {
            chargeTimer += TimeManager.Instance.PlayerDelta;

            if (chargeTimer > chargeTime)
            {
                currentStack++;
                chargeTimer = 0f;
            }
        }
    }

    public void OnDash(InputValue value)
    {
        if (playerState.IsDashing) return;
        if (playerState.IsDead) return;
        if (playerState.IsUsingSkill) return;
        if (currentStack < 1 || reuseTimer > 0) return;

        if (playerState.move.moveDirection.sqrMagnitude > 0f)
        {
            dashDirection = playerState.move.moveDirection;
            transform.rotation = Quaternion.LookRotation(dashDirection);
        }
        else
        {
            dashDirection = transform.forward;
        }
        bool perfectDodge = dodgeDetector.isEnemyAttackNearby && !playerState.isHit;

        if (perfectDodge)
        {
            TimeManager.Instance.BulletTime(playerState.animator);
        }

        currentStack--;
        reuseTimer = reuseCooldown;
        playerState.ChangeState(playerState.dashState);
    }

    public IEnumerator DashCoroutine()
    {
        // yield return new WaitForSecondsRealtime(dashDuration);
        //
        // if (playerState.move.moveDirection.sqrMagnitude > 0f)
        // {
        //     playerState.ChangeState(playerState.sprintState);
        // }
        // else
        // {
        //     playerState.ChangeState(playerState.idleState);
        // }
        //
        // yield return new WaitForSecondsRealtime(0.1f);
        // playerState.canAttack = true;

        float dashDistance = dashSpeed * dashDuration;
        float moved = 0;
        while (moved < dashDistance)
        {
            float step = dashSpeed * TimeManager.Instance.PlayerDelta;
            playerState.characterController.Move(dashDirection * step);
            moved += step;
            yield return null;
        }

        if (playerState.move.moveDirection.sqrMagnitude > 0f)
        {
            playerState.ChangeState(playerState.sprintState);
        }
        else
        {
            playerState.ChangeState(playerState.idleState);
        }

        yield return new WaitForSecondsRealtime(0.1f);
        playerState.canAttack = true;
    }
}