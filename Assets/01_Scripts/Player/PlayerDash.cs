using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour, IBattleComponent
{
    // 참조
    private PlayerStateManager player;
    private DodgeDetector dodgeDetector;

    // 대쉬 설정
    [SerializeField] private float dashDuration = 0.4f;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private float reuseCooldown = 0.8f;
    public float dashSpeed = 8f;

    // 대쉬 스택
    private int maxStack = 3;
    private int currentStack = 3;
    private float chargeTimer;
    private float reuseTimer;

    // 런타임
    [HideInInspector] public Vector3 dashDirection;

    // UI용 프로퍼티
    public int CurrentStack => currentStack;
    public float ChargeFillAmount => chargeTimer / chargeTime;
    public bool IsReuseDelay => reuseTimer > 0f;
    public float ReuseTimer => reuseTimer;

    private void Awake()
    {
        player = GetComponent<PlayerStateManager>();
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
        if (player.IsDashing) return;
        if (player.IsDead) return;
        if (player.IsUsingSkill) return;
        if (currentStack < 1 || reuseTimer > 0) return;

        if (player.move.moveDirection.sqrMagnitude > 0f)
        {
            dashDirection = player.move.moveDirection;
            transform.rotation = Quaternion.LookRotation(dashDirection);
        }
        else
        {
            dashDirection = transform.forward;
        }
        bool perfectDodge = dodgeDetector.isEnemyAttackNearby && !player.isHit;

        if (perfectDodge)
        {
            TimeManager.Instance.BulletTime(player.animator);
        }

        currentStack--;
        reuseTimer = reuseCooldown;
        player.ChangeState(player.dashState);
    }

    public IEnumerator DashCoroutine()
    {
        float dashDistance = dashSpeed * dashDuration;
        float moved = 0;
        while (moved < dashDistance)
        {
            float step = dashSpeed * TimeManager.Instance.PlayerDelta;
            player.characterController.Move(dashDirection * step);
            moved += step;
            yield return null;
        }

        if (player.move.moveDirection.sqrMagnitude > 0f)
        {
            player.ChangeState(player.sprintState);
        }
        else
        {
            player.ChangeState(player.idleState);
        }

        yield return new WaitForSecondsRealtime(0.1f);
        player.canAttack = true;
    }
}