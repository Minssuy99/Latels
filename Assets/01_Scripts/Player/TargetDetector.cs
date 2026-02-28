using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour, IBattleComponent
{
    public float LastDistance { get; private set; } = Mathf.Infinity;

    public List<EnemyStateManager> Enemies { get; private set; } = new ();

    public EnemyStateManager FindNearestTarget(GameObject exclude = null)
    {
        EnemyStateManager nearestTarget = null;
        float nearest = Mathf.Infinity;
        float distance;

        foreach (var enemy in Enemies)
        {
            if (enemy == null) continue;
            if (enemy.gameObject == exclude)  continue;
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

    public void SetEnemies(List<EnemyStateManager> enemies)
    {
        Enemies = enemies;
    }
}