using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RuruneAttack : PlayerAttack, IBattleComponent
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] muzzlePositions;
    [SerializeField] private float animRigChangeSpeed = 10f;
    [SerializeField] private float meleeHitRange = 0.1f;

    private Rig aimRig;
    private int muzzleIndex;
    private ParticleSystem[] muzzleEffects;
    public Transform[] MuzzlePosition => muzzlePositions;
    private float lastShootTime;
    private int count;

    protected override void Awake()
    {
        base.Awake();
        aimRig = GetComponentInChildren<Rig>();

        muzzleEffects = new ParticleSystem[muzzlePositions.Length];
        for (int i = 0; i < muzzleEffects.Length; i++)
        {
            muzzleEffects[i] = muzzlePositions[i].GetComponentInChildren<ParticleSystem>();
        }
    }

    public void Shoot()
    {
        if (player.targetEnemy == null) return;

        if (Time.unscaledTime - lastShootTime < 0.05f) return;
        lastShootTime = Time.unscaledTime;

        if (player.targetDistance < meleeHitRange)
        {
            player.targetEnemy.GetComponent<IDamageable>().TakeDamage(player.CharacterData.stats.damage, transform.position);
        }

        count++;
        player.animator.SetInteger(AnimHash.AttackCount, count);

        muzzleEffects[muzzleIndex].Play();

        GameObject bullet = PoolManager.Instance.Get(bulletPrefab);
        bullet.transform.position = muzzlePositions[muzzleIndex].position;
        bullet.GetComponent<Bullet>().Init(player.targetEnemy.transform, player.CharacterData.stats.damage, transform.forward);

        muzzleIndex = (muzzleIndex + 1) % muzzlePositions.Length;
    }

    public override void ExecuteAttack()
    {
        if (count >= 12) return;
        player.animator.SetTrigger(AnimHash.Attack);
    }

    public void Reload()
    {
        count = 0;
        player.animator.SetInteger(AnimHash.AttackCount, 0);
    }

    public override bool OnTargetLost()
    {
        player.SetIsAttacking(false);
        player.SetIsAttackFinishing(false);
        player.animator.ResetTrigger(AnimHash.Attack);
        return true;
    }

    private void LateUpdate()
    {
        bool shouldAim = (player.isAttacking || player.isAttackFinishing) && player.move.MoveDirection.sqrMagnitude > 0;

        if (shouldAim)
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 1, animRigChangeSpeed * TimeManager.Instance.PlayerDelta);
        }
        else
        {
            aimRig.weight = Mathf.Lerp(aimRig.weight, 0, animRigChangeSpeed * TimeManager.Instance.PlayerDelta);
        }
    }
}