using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    private Bloom bloom;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 20f;

    private PlayerStateManager playerState;

    private Vector3 _moveDirection;
    private Vector3 _dashDirection;
    private Vector3 _lastDirection;
    private Quaternion _targetRotation;
    private bool _wasDiagonal;

    private float _xValue;
    private float _zValue;
    private float _velocityXSmooth = 0f;
    private float _velocityZSmooth = 0f;

    [SerializeField] private DodgeDetector dodgeDetector;
    [SerializeField] private float dashCoolTime = 3f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashSpeed = 10f;
    private float baseFixedDeltaTime;
    private bool canDash = true;

    private float groundY;

    private void Awake()
    {
        baseFixedDeltaTime = Time.fixedDeltaTime;
        playerState = GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        groundY = transform.position.y;

        globalVolume.profile.TryGet(out bloom);
    }

    public void OnMove(InputValue value)
    {
        _xValue = value.Get<Vector2>().x;
        _zValue = value.Get<Vector2>().y;
    }

    public void OnDash(InputValue value)
    {
        if (playerState.isDashing) return;
        if (playerState.isDead) return;
        if (!canDash) return;

        if (_moveDirection.sqrMagnitude > 0f)
        {
            _dashDirection = _moveDirection;
            transform.rotation = Quaternion.LookRotation(_dashDirection);
        }
        else
        {
            _dashDirection = transform.forward;
        }
        bool perfectDodge = dodgeDetector.isEnemyAttackNearby && !playerState.isHit;

        if (perfectDodge)
        {
            StartCoroutine(BulletTimeCoroutine());
        }
        StartCoroutine(DashCoroutine(perfectDodge));
    }

    private void Update()
    {
        if (playerState.isDead) return;
        if (playerState.isUsingMainSkill) return;

        if (playerState.characterController.isGrounded)
        {
            groundY = transform.position.y;
        }

        HandleMovement();
        HandleRotation();
        UpdateAnimParameter();
        UpdateAttackLayers();
        UpdateHitLayers();
        ApplyRotation();
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float groundY = transform.position.y;

        if (hit.gameObject.CompareTag("Enemy"))
        {
            if (transform.position.y > groundY + 0.1f)
            {
                playerState.characterController.enabled = false;
                Vector3 pos = transform.position;
                pos.y = groundY;
                transform.position = pos;
                playerState.characterController.enabled = true;
            }
        }
    }

    IEnumerator DashCoroutine(bool perfectDodge)
    {
        canDash = false;
        playerState.canAttack = false;

        playerState.isDashing = true;
        playerState.isAttacking = false;
        playerState.isHit = false;
        playerState.animator.SetLayerWeight(1, 0f);
        playerState.animator.SetLayerWeight(2, 0f);
        playerState.animator.SetLayerWeight(3, 0f);
        playerState.animator.SetLayerWeight(4, 0f);
        playerState.animator.SetTrigger("Dash");

        yield return new WaitForSecondsRealtime(dashDuration);

        playerState.isDashing = false;

        yield return new WaitForSecondsRealtime(0.1f);
        playerState.canAttack = true;

        yield return new WaitForSecondsRealtime(dashCoolTime - 0.1f);
        canDash = true;
    }

    IEnumerator BulletTimeCoroutine()
    {
        float slowScale = 0.1f;

        bloom.tint.value = Color.red;

        Debug.Log("불렛타임 시작!");  // 추가

        Time.timeScale = slowScale;
        Time.fixedDeltaTime = baseFixedDeltaTime * slowScale;
        playerState.animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        yield return new WaitForSecondsRealtime(4f);

        Debug.Log("불렛타임 끝!");  // 추가
        Time.timeScale = 1f;
        Time.fixedDeltaTime = baseFixedDeltaTime;
        playerState.animator.updateMode = AnimatorUpdateMode.Normal;

        bloom.tint.value = Color.white;
    }

    private void HandleMovement()
    {
        _moveDirection = new Vector3(_xValue, 0, _zValue);

        if (playerState.isDashing)
        {
            playerState.characterController.Move(_dashDirection * (dashSpeed * Time.unscaledDeltaTime));
        }
        else if (_moveDirection.sqrMagnitude > 0f)
        {
            CheckDirection();
            playerState.characterController.Move(_lastDirection * (moveSpeed * Time.unscaledDeltaTime));
        }

    }

    private void HandleRotation()
    {
        if(playerState.isDashing) return;

        if (playerState.isLockedOn && playerState.targetEnemy != null)
        {
            Vector3 enemyDirection = playerState.targetEnemy.transform.position - transform.position;
            enemyDirection.y = 0f;
            _targetRotation = Quaternion.LookRotation(enemyDirection);
        }
        else
        {
            if (_moveDirection.sqrMagnitude > 0f)
            {
                _targetRotation = Quaternion.LookRotation(_moveDirection);
            }
        }
    }

    private void UpdateAnimParameter()
    {
        if (playerState.isDashing) return;

        if (playerState.isLockedOn)
        {
            Vector3 worldInput = new Vector3(_xValue, 0, _zValue);

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
            float smoothX = Mathf.SmoothDamp(currentX, targetX, ref _velocityXSmooth, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
            float smoothZ = Mathf.SmoothDamp(currentZ, targetZ, ref _velocityZSmooth, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);

            playerState.animator.SetFloat("VelocityX", smoothX);
            playerState.animator.SetFloat("VelocityZ", smoothZ);
            playerState.animator.SetFloat("Velocity", _moveDirection.magnitude);
        }
        else
        {
            playerState.animator.SetLayerWeight(1, 0.0f);
            playerState.animator.SetLayerWeight(2, 0.0f);
            playerState.animator.SetFloat("Velocity", _moveDirection.magnitude);
        }
    }

    private void UpdateAttackLayers()
    {
        if (playerState.isDashing) return;

        if (playerState.isAttacking)
        {
            bool isMoving = _moveDirection.sqrMagnitude > 0.1f;

            if (isMoving)
            {
                playerState.animator.SetLayerWeight(1, 0.0f);
                playerState.animator.SetLayerWeight(2, 1.0f);
            }
            else
            {
                playerState.animator.SetLayerWeight(1, 1.0f);
                playerState.animator.SetLayerWeight(2, 0.0f);
            }
        }
        else
        {
            playerState.animator.SetLayerWeight(1, 0.0f);
            playerState.animator.SetLayerWeight(2, 0.0f);
        }
    }

    private void UpdateHitLayers()
    {
        if(playerState.isDashing) return;

        if (playerState.isHit)
        {
            bool isMoving = _moveDirection.sqrMagnitude > 0.1f;

            if (isMoving || playerState.isAttacking)
            {
                playerState.animator.SetLayerWeight(3, 0f);
                playerState.animator.SetLayerWeight(4, 1f);
            }
            else
            {
                playerState.animator.SetLayerWeight(3, 1f);
                playerState.animator.SetLayerWeight(4, 0f);
            }
        }
        else
        {
            playerState.animator.SetLayerWeight(3, 0f);
            playerState.animator.SetLayerWeight(4, 0f);
        }
    }

    private void ApplyRotation()
    {
        if (playerState.isDashing) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.unscaledDeltaTime);
    }

    private void CheckDirection()
    {
        bool isDiagonal = (_xValue != 0 && _zValue != 0);
        bool isSingleDirection = (_xValue != 0 || _zValue != 0) && !isDiagonal;

        if (isDiagonal)
        {
            _lastDirection = _moveDirection;
        }
        else if (isSingleDirection && !_wasDiagonal)
        {
            _lastDirection = _moveDirection;
        }

        _wasDiagonal = isDiagonal;
    }
}
