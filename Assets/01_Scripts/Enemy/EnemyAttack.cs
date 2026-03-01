using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    protected EnemyStateManager enemy;
    public bool superArmor { get; set; }
    public int attackType { get; set; }
    public float attackCooldown { get; set; }

    protected virtual void Awake()
    {
        enemy = GetComponent<EnemyStateManager>();
    }

    protected virtual void Start()
    {
        attackCooldown = enemy.Data.stats.attackCooldown;
    }

    public abstract void DisableAllHitboxes();

    public abstract void SetHitbox(int action);

    public abstract void SetDangerZone(int action);

    protected static void SetColliders(GameObject[] objects, bool active)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(active);
        }
    }
}
