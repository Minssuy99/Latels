using UnityEngine;

public class DodgeDetector : MonoBehaviour
{
    public bool isEnemyAttackNearby { get; private set; } = false;

    [SerializeField] private float detectionDuration = 0.3f;
    private float detectionTimer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyDangerZone"))
        {
            detectionTimer = detectionDuration;
        }
    }

    private void Update()
    {
        if (detectionTimer > 0)
        {
            isEnemyAttackNearby = true;
            detectionTimer -= TimeManager.Instance.PlayerDelta;
        }
        else
        {
            isEnemyAttackNearby = false;
        }
    }
}
