using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    // 참조
    private PlayerStateManager playerState;
    [SerializeField] private DodgeDetector dodgeDetector;

    // 대쉬 설정
    [SerializeField] private float dashCoolTime = 3f;
    [SerializeField] private float dashDuration = 0.4f;
    public float dashSpeed = 8f;

    // 런타임
    [HideInInspector] public Vector3 dashDirection;
    [HideInInspector] public bool canDash = true;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    public void OnDash(InputValue value)
    {
        if (playerState.IsDashing) return;
        if (playerState.IsDead) return;
        if (!canDash) return;

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
            StartCoroutine(BulletTimeManager.Instance.StartBulletTime(playerState.animator));
        }
        playerState.ChangeState(playerState.dashState);
    }

    public IEnumerator DashCoroutine()
    {
        yield return new WaitForSecondsRealtime(dashDuration);

        playerState.ChangeState(playerState.idleState);

        yield return new WaitForSecondsRealtime(0.1f);
        playerState.canAttack = true;

        yield return new WaitForSecondsRealtime(dashCoolTime - 0.1f);
        canDash = true;
    }
}