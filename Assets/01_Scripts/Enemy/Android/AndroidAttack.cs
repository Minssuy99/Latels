using UnityEngine;

public class AndroidAttack : EnemyAttack
{
    [Header("※ Hitbox")]
    [SerializeField] private GameObject[] PunchHitboxes;
    [SerializeField] private GameObject[] KickHitboxes;
    [Header("※ Danger Zone")]
    [SerializeField] private GameObject[] PunchDangerZones;
    [SerializeField] private GameObject[] KickDangerZones;

    public override void DisableAllHitboxes()
    {
        SetColliders(PunchHitboxes, false);
        SetColliders(KickHitboxes, false);
        SetColliders(PunchDangerZones, false);
        SetColliders(KickDangerZones, false);
    }

    public override void SetHitbox(int action)
    {
        switch (action)
        {
            case 0: SetColliders(PunchHitboxes, true); break;
            case 1: SetColliders(PunchHitboxes, false); break;
            case 2: SetColliders(KickHitboxes, true); break;
            case 3: SetColliders(KickHitboxes, false); break;
        }
    }

    public override void SetDangerZone(int action)
    {
        switch (action)
        {
            case 0: SetColliders(PunchDangerZones, true); break;
            case 1: SetColliders(PunchDangerZones, false); break;
            case 2: SetColliders(KickDangerZones, true); break;
            case 3: SetColliders(KickDangerZones, false); break;
        }
    }
}