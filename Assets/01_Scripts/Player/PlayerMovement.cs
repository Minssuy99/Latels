using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IBattleComponent
{
    private PlayerStateManager player;

    private VariableJoystick joystick;
    [SerializeField] private float moveSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float rotationSpeed = 15f;
    public Vector3 MoveDirection { get; private set; }
    private Vector3 lastDirection;
    private Quaternion targetRotation;
    private bool wasDiagonal;

    private Vector2 keyboardInput;
    private float velocityXSmooth;
    private float velocityZSmooth;

    private float groundY;

    private void Awake()
    {
        player = GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        groundY = transform.position.y;
    }

    private void Update()
    {
        if (joystick && joystick.Direction.sqrMagnitude > 0.01f)
        {
            Vector2 dir = joystick.Direction.normalized;
            MoveDirection = new Vector3(dir.x, 0, dir.y);
        }
        else
        {
            MoveDirection = new Vector3(keyboardInput.x, 0, keyboardInput.y);
        }

        if (player.characterController.isGrounded)
        {
            groundY = transform.position.y;
        }
    }

    private void LateUpdate()
    {
        if (transform.position.y > groundY + 0.05f && !player.characterController.isGrounded)
        {
            Vector3 pos = transform.position;
            pos.y = groundY;

            player.characterController.enabled = false;
            transform.position = pos;
            player.characterController.enabled = true;
        }
    }

    public void OnMove(InputValue value)
    {
        keyboardInput = value.Get<Vector2>();
    }

    public void HandleMovement(bool isRunning = false)
    {
        if (MoveDirection.sqrMagnitude > 0f)
        {
            CheckDirection();
            float speed = isRunning ? sprintSpeed : moveSpeed;
            player.characterController.Move(lastDirection * (speed * TimeManager.Instance.PlayerDelta));
        }
    }

    public void HandleRotation()
    {
        if (player.IsDashing) return;

        if (player.isLockedOn && player.targetEnemy)
        {
            Vector3 enemyDirection = player.targetEnemy.transform.position - transform.position;
            enemyDirection.y = 0f;
            targetRotation = Quaternion.LookRotation(enemyDirection);
        }
        else
        {
            if (MoveDirection.sqrMagnitude > 0f)
            {
                targetRotation = Quaternion.LookRotation(MoveDirection);
            }
        }
    }
    public void UpdateAnimParameter()
    {
        if (player.isLockedOn)
        {
            Vector3 worldInput = MoveDirection;

            if (worldInput.sqrMagnitude > 0.1f)
            {
                worldInput = worldInput.normalized;
            }

            Vector3 localInput = transform.InverseTransformDirection(worldInput);

            float targetX = localInput.x;
            float targetZ = localInput.z;

            float currentX = player.animator.GetFloat(AnimHash.VelocityX);
            float currentZ = player.animator.GetFloat(AnimHash.VelocityZ);

            float smoothTime = 0.1f;
            float smoothX = Mathf.SmoothDamp(currentX, targetX, ref velocityXSmooth, smoothTime, Mathf.Infinity, TimeManager.Instance.PlayerDelta);
            float smoothZ = Mathf.SmoothDamp(currentZ, targetZ, ref velocityZSmooth, smoothTime, Mathf.Infinity, TimeManager.Instance.PlayerDelta);

            player.animator.SetFloat(AnimHash.VelocityX, smoothX);
            player.animator.SetFloat(AnimHash.VelocityZ, smoothZ);
            player.animator.SetFloat(AnimHash.Velocity, MoveDirection.magnitude);
        }
        else
        {
            player.animator.SetFloat(AnimHash.Velocity, MoveDirection.magnitude);
        }
    }

    public void ApplyRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * TimeManager.Instance.PlayerDelta);
    }

    public void CheckDirection()
    {
        bool isDiagonal = (MoveDirection.x != 0 && MoveDirection.z != 0);
        bool isSingleDirection = (MoveDirection.x != 0 || MoveDirection.z != 0) && !isDiagonal;

        if (isDiagonal)
        {
            lastDirection = MoveDirection;
        }
        else if (isSingleDirection && !wasDiagonal)
        {
            lastDirection = MoveDirection;
        }
        wasDiagonal = isDiagonal;
    }

    public void SetJoystick(VariableJoystick joystick)
    {
        this.joystick = joystick;
    }
}
