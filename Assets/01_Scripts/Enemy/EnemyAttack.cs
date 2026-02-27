using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("※ Hitbox")]
    [SerializeField] private GameObject[] PunchHitboxes;
    [SerializeField] private GameObject[] KickHitboxes;
    [Header("※ Danger Zone")]
    [SerializeField] private GameObject[] PunchDangerZones;
    [SerializeField] private GameObject[] KickDangerZones;

    private EnemyStateManager enemy;
    public bool superArmor { get; set; }
    public int attackType { get; set; }
    public float attackCooldown { get; set; }

    private void Awake()
    {
        enemy = GetComponent<EnemyStateManager>();
    }

    private void Start()
    {
        attackCooldown = enemy.Data.stats.attackCooldown;
    }

    public void DisableAllHitboxes()
    {
        SetColliders(PunchHitboxes, false);
        SetColliders(KickHitboxes, false);
        SetColliders(PunchDangerZones, false);
        SetColliders(KickDangerZones, false);
    }

    public void SetHitbox(int action)
    {
        switch (action)
        {
            case 0: SetColliders(PunchHitboxes, true); break;
            case 1: SetColliders(PunchHitboxes, false); break;
            case 2: SetColliders(KickHitboxes, true); break;
            case 3: SetColliders(KickHitboxes, false); break;
        }
    }

    public void SetDangerZone(int action)
    {
        switch (action)
        {
            case 0: SetColliders(PunchDangerZones, true); break;
            case 1: SetColliders(PunchDangerZones, false); break;
            case 2: SetColliders(KickDangerZones, true); break;
            case 3: SetColliders(KickDangerZones, false); break;
        }
    }

    private void SetColliders(GameObject[] objects, bool active)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(active);
        }
    }
}
