using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 20f;

    private PlayerStateManager playerState;

    private Vector3 _moveDirection;
    private Vector3 _lastDirection;
    private Quaternion _targetRotation;
    private bool _wasDiagonal;

    private float _xValue;
    private float _zValue;
    private float _velocityXSmooth = 0f;
    private float _velocityZSmooth = 0f;

    private void Start()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    public void OnMove(InputValue value)
    {
        _xValue = value.Get<Vector2>().x;
        _zValue = value.Get<Vector2>().y;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        UpdateAnimParameter();
        UpdateAttackLayers();
        ApplyRotation();
    }

    private void HandleMovement()
    {
        _moveDirection = new Vector3(_xValue, 0, _zValue);
        if (_moveDirection.sqrMagnitude > 0f)
        {
            CheckDirection();
            playerState.characterController.Move(_lastDirection * (moveSpeed * Time.deltaTime));
        }
    }

    private void HandleRotation()
    {
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
            float smoothX = Mathf.SmoothDamp(currentX, targetX, ref _velocityXSmooth, smoothTime);
            float smoothZ = Mathf.SmoothDamp(currentZ, targetZ, ref _velocityZSmooth, smoothTime);

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

    private void ApplyRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
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
