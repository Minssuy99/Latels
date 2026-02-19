using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStateManager playerState;

    // 이동 설정
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float SprintSpeed = 7f;
    [SerializeField] private float rotationSpeed = 20f;

    // 이동 상태
    [HideInInspector] public Vector3 moveDirection;
    private Vector3 lastDirection;
    private Quaternion targetRotation;
    private bool wasDiagonal;

    // 애니메이션
    private float velocityXSmooth = 0f;
    private float velocityZSmooth = 0f;

    // 충돌 보정
    private float groundY;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        groundY = transform.position.y;
    }

    private void Update()
    {
        if (playerState.characterController.isGrounded)
        {
            groundY = transform.position.y;
        }
    }

    private void LateUpdate()
    {
        if (transform.position.y > groundY + 0.05f && !playerState.characterController.isGrounded)
        {
            Vector3 pos = transform.position;
            pos.y = groundY;

            playerState.characterController.enabled = false;
            transform.position = pos;
            playerState.characterController.enabled = true;
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y);
    }

    public void HandleMovement(bool isRunning = false)
    {
        if (moveDirection.sqrMagnitude > 0f)
        {
            CheckDirection();
            float speed = isRunning ? SprintSpeed : moveSpeed;
            playerState.characterController.Move(lastDirection * (speed * TimeManager.Instance.PlayerDelta));
        }
    }

    public void HandleRotation()
    {
        if (playerState.isLockedOn && playerState.targetEnemy != null)
        {
            Vector3 enemyDirection = playerState.targetEnemy.transform.position - transform.position;
            enemyDirection.y = 0f;
            targetRotation = Quaternion.LookRotation(enemyDirection);
        }
        else
        {
            if (moveDirection.sqrMagnitude > 0f)
            {
                targetRotation = Quaternion.LookRotation(moveDirection);
            }
        }
    }

    public void UpdateAnimParameter()
    {
        if (playerState.isLockedOn)
        {
            Vector3 worldInput = moveDirection;

            if (worldInput.sqrMagnitude > 0.1f)
            {
                worldInput = worldInput.normalized;
            }

            Vector3 localInput = transform.InverseTransformDirection(worldInput);

            float targetX = localInput.x;
            float targetZ = localInput.z;

            float currentX = playerState.animator.GetFloat("VelocityX");
            float currentZ = playerState.animator.GetFloat("VelocityZ");

            float smoothTime = 0.1f;
            float smoothX = Mathf.SmoothDamp(currentX, targetX, ref velocityXSmooth, smoothTime, Mathf.Infinity, TimeManager.Instance.PlayerDelta);
            float smoothZ = Mathf.SmoothDamp(currentZ, targetZ, ref velocityZSmooth, smoothTime, Mathf.Infinity, TimeManager.Instance.PlayerDelta);

            playerState.animator.SetFloat("VelocityX", smoothX);
            playerState.animator.SetFloat("VelocityZ", smoothZ);
            playerState.animator.SetFloat("Velocity", moveDirection.magnitude);
        }
        else
        {
            playerState.animator.SetLayerWeight(1, 0.0f);
            playerState.animator.SetLayerWeight(2, 0.0f);
            playerState.animator.SetFloat("Velocity", moveDirection.magnitude);
        }
    }

    public void ApplyRotation()
    {
        if (playerState.animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * TimeManager.Instance.PlayerDelta);
    }

    public void CheckDirection()
    {
        bool isDiagonal = (moveDirection.x != 0 && moveDirection.z != 0);
        bool isSingleDirection = (moveDirection.x != 0 || moveDirection.z != 0) && !isDiagonal;

        if (isDiagonal)
        {
            lastDirection = moveDirection;
        }
        else if (isSingleDirection && !wasDiagonal)
        {
            lastDirection = moveDirection;
        }
        wasDiagonal = isDiagonal;
    }
}
