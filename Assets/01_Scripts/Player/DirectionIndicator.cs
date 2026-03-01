using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    private PlayerStateManager player;
    private Quaternion currentRotation;

    private void Awake()
    {
        player = GetComponentInParent<PlayerStateManager>();
    }

    private void Start()
    {
        currentRotation = Quaternion.LookRotation(player.transform.forward);
    }

    private void LateUpdate()
    {
        Vector3 dir = Vector3.zero;

        if (player.move.MoveDirection.sqrMagnitude > 0)
        {
            dir = player.move.MoveDirection;
        }
        else if (player.targetEnemy && player.isLockedOn)
        {
            dir = player.targetEnemy.transform.position - player.transform.position;
        }
        else
        {
            dir = player.transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * TimeManager.Instance.PlayerDelta);

        float yAngle = currentRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yAngle, 0);
    }
}