using UnityEngine;

public class DodgeDetector : MonoBehaviour
{
    public bool isEnemyAttackNearby { get; private set; }

    [SerializeField] private float detectionDuration = 0.3f;
    private float detectionTimer;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(GameTags.EnemyDangerZone))
        {
            detectionTimer = detectionDuration;
        }
    }

    private void Update()
    {
        detectionTimer -= TimeManager.Instance.PlayerDelta;
        isEnemyAttackNearby = detectionTimer > 0 && TimeManager.Instance.IsNormalMotion;
    }
}
