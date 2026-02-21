using UnityEngine;
using System.Collections;

public class RuruneSkill : PlayerSkill
{
    [SerializeField] private float range;
    [SerializeField] private float damage;

    private IEnumerator SkillSequence()
    {
        player.animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        GameObject target1 = FindNearestEnemy(null);

        if (target1 != null)
        {
            transform.LookAt(target1.transform);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        player.animator.SetTrigger("Kick1");
        yield return new WaitForSecondsRealtime(2.3f);

        GameObject target2 = FindNearestEnemy(target1);
        if (target2 == null && target1 != null && target1.GetComponent<EnemyStateManager>().attack.HP > 0)
        {
            target2 = target1;
        }

        if (target2 != null)
        {
            transform.LookAt(target2.transform);
        }

        if (target2 == null)
        {
            player.animator.updateMode = AnimatorUpdateMode.Normal;
            yield break;
        }
        player.animator.SetTrigger("Kick2");
        yield return new WaitForSecondsRealtime(3.5f);
        player.animator.updateMode = AnimatorUpdateMode.Normal;
    }

    public void Rurune_Attack()
    {
        float dist;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null) continue;
            if (enemies[i].attack.HP <= 0) continue;

            dist = Vector3.Distance(transform.position, enemies[i].transform.position);
            if (dist <= range)
            {
                enemies[i].attack.TakeDamage(damage);
            }
        }
    }

    public override void OnSkillStart()
    {
        base.OnSkillStart();
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        yield return StartCoroutine(SkillSequence());
        if(player.setup.Role == CharacterRole.Support)
            gameObject.SetActive(false);
    }
}
