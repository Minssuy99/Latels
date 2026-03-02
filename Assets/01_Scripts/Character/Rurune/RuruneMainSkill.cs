using UnityEngine;
using System.Collections;

public class RuruneMainSkill : PlayerMainSkill
{
    [SerializeField] private float range;
    [SerializeField] private int effectCount = 3;
    [SerializeField] private GameObject[] muzzleEffects;
    [SerializeField] private GameObject[] explosionEffects;

    private RuruneAttack attack;
    private Collider[] skillHitBuffer = new Collider[20];

    protected override void Awake()
    {
        base.Awake();
        attack = GetComponent<RuruneAttack>();
    }

    private IEnumerator SkillSequence()
    {
        GameObject target = targetDetector.FindNearestTarget()?.gameObject;

        if (player.move.MoveDirection.sqrMagnitude > 0)
            transform.rotation = Quaternion.LookRotation(player.move.MoveDirection);
        else if (target)
            transform.LookAt(target.transform);

        yield return TimeManager.Instance.WaitRealTime(0.9f);

        target = targetDetector.FindNearestTarget()?.gameObject;
        if (player.move.MoveDirection.sqrMagnitude > 0)
            transform.rotation = Quaternion.LookRotation(player.move.MoveDirection);
        else if (target)
            transform.LookAt(target.transform);
    }

    public void OnRuruneSkill()
    {
        DealDamageInRange();
        SpawnExplosionEffect();
        SpawnMuzzleEffect();
    }

    private void DealDamageInRange()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, range, skillHitBuffer);
        for (int i = 0; i < count; i++)
        {
            EnemyStateManager enemy = skillHitBuffer[i].GetComponent<EnemyStateManager>();
            if (enemy == null) continue;
            if (enemy.currentState is EnemyDeadState) continue;

            enemy.GetComponent<IDamageable>().TakeDamage(player.SkillDamage, transform.position);
        }
    }

    private void SpawnExplosionEffect()
    {
        for (int i = 0; i < effectCount; i++)
        {
            int explosionIndex = Random.Range(0, explosionEffects.Length);
            GameObject explosionEffect = PoolManager.Instance.Get(explosionEffects[explosionIndex]);
            Vector2 randomPos = Random.insideUnitCircle * range;
            explosionEffect.transform.position = new Vector3(transform.position.x + randomPos.x, transform.position.y + Vector3.up.y, transform.position.z + randomPos.y);
            TimeManager.Instance.PlayParticle(explosionEffect);
            PoolManager.Instance.Return(explosionEffect, 5f);
        }
    }

    private void SpawnMuzzleEffect()
    {
        foreach (var muzzlePos in attack.MuzzlePosition)
        {
            int muzzleIndex = Random.Range(0, muzzleEffects.Length);
            GameObject muzzleEffect = PoolManager.Instance.Get(muzzleEffects[muzzleIndex]);
            muzzleEffect.transform.position = muzzlePos.position;
            TimeManager.Instance.PlayParticle(muzzleEffect);
            PoolManager.Instance.Return(muzzleEffect, 5f);
        }
    }

    public override void OnSkillStart()
    {
        base.OnSkillStart();
        StartCoroutine(SkillSequence());
    }

    public void OnSkillEnd()
    {
        if (StageManager.Instance.IsClear)
        {
            player.animator.CrossFade(AnimHash.Idle, 0.2f);
            EndSkill();
            return;
        }

        if (player.dash.ConsumeDashBuffer())
        {
            player.animator.Play(AnimHash.Dash, 0, 0);
            player.ChangeState(player.dashState);
            return;
        }

        if (player.isLockedOn)
            player.animator.CrossFade(AnimHash.LockOn, 0.2f, 0);
        else if (player.move.MoveDirection.sqrMagnitude > 0)
            player.animator.CrossFade(AnimHash.Run, 0.2f, 0);
        else
            player.animator.CrossFade(AnimHash.Idle, 0.2f, 0);

        EndSkill();
    }
}