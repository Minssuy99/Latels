using UnityEngine;
using System.Collections;

public class RuruneMainSkill : PlayerMainSkill
{
    private RuruneAttack ruruneAttack;

    private IEnumerator SkillSequence()
    {
        player.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        GameObject target1 = targetDetector.FindNearestTarget()?.gameObject;

        if (player.move.moveDirection.sqrMagnitude > 0)
            transform.rotation = Quaternion.LookRotation(player.move.moveDirection);
        else if (target1)
            transform.LookAt(target1.transform);

        yield return new WaitForSecondsRealtime(0.1f);
        player.animator.SetTrigger("Kick1");
        yield return new WaitForSecondsRealtime(0.8f);

        GameObject target2 = targetDetector.FindNearestTarget(target1)?.gameObject;
        if (!target2 && target1 && target1.GetComponent<EnemyStateManager>().health.HP > 0)
        {
            target2 = target1;
        }

        if (player.move.moveDirection.sqrMagnitude > 0)
            transform.rotation = Quaternion.LookRotation(player.move.moveDirection);
        else if (target2)
            transform.LookAt(target2.transform);

        yield return new WaitForSecondsRealtime(0.1f);
        player.animator.SetTrigger("Kick2");
        yield return new WaitForSecondsRealtime(1.675f);
        player.animator.updateMode = AnimatorUpdateMode.Normal;
    }

    public void OnRuruneSkill()
    {
        if (ruruneAttack == null)
            ruruneAttack = GetComponent<RuruneAttack>();
        ruruneAttack.Shoot();
    }

    public override void OnSkillStart()
    {
        base.OnSkillStart();
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        yield return StartCoroutine(SkillSequence());
        EndSkill();
    }
}