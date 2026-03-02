using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    protected EnemyStateManager enemy;
    public bool SuperArmor { get; private set; }
    public int AttackType { get; set; }
    public float AttackCooldown { get; private set; }
    public bool IsReady => AttackCooldown <= 0;

    protected virtual void Awake()
    {
        enemy = GetComponent<EnemyStateManager>();
    }

    protected virtual void Start()
    {
        AttackCooldown = enemy.Data.stats.attackCooldown;
    }

    public void ResetAfterAttack()
    {
        AttackCooldown = Random.Range(1, enemy.AttackCooldown);
        SuperArmor = false;
    }

    public void TickCooldown()
    {
        AttackCooldown -= TimeManager.Instance.EnemyDelta;
    }

    public void ActivateSuperArmor()
    {
        SuperArmor = true;
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
