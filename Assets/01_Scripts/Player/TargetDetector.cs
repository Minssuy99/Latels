using UnityEngine;

public class TargetDetector : MonoBehaviour, IBattleComponent
{
    [SerializeField] private float detectRange;
    public float LastDistance { get; private set; } = Mathf.Infinity;

    private Collider[] hitBuffer = new Collider[20];

    public EnemyStateManager FindNearestTarget(GameObject exclude = null)
    {
        EnemyStateManager nearestTarget = null;
        float nearest = Mathf.Infinity;
        float distance;

        int count = Physics.OverlapSphereNonAlloc(transform.position, detectRange, hitBuffer);

        for (int i = 0; i < count; i++)
        {
            EnemyStateManager enemy = hitBuffer[i].GetComponent<EnemyStateManager>();
            if (!enemy) continue;
            if (enemy.gameObject == exclude) continue;
            if (enemy.currentState is EnemyInactiveState) continue;
            if (enemy.currentState is EnemyDeadState) continue;

            distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearest)
            {
                nearest = distance;
                nearestTarget = enemy;
            }
        }
        LastDistance = nearestTarget? nearest : Mathf.Infinity;
        return nearestTarget;
    }
}