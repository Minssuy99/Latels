using System.Collections;
using UnityEngine;

public class Rurune : SupportCharacter
{
    [SerializeField] private float range;
    [SerializeField] private float damage;

    protected override IEnumerator SkillSequence()
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        GameObject target1 = FindNearestEnemy(null);

        if (target1 != null)
        {
            transform.LookAt(target1.transform);
        }
        animator.SetTrigger("Kick1");
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

        animator.SetTrigger("Kick2");
        yield return new WaitForSecondsRealtime(3.5f);
        animator.updateMode = AnimatorUpdateMode.Normal;
        gameObject.SetActive(false);
    }

    private GameObject FindNearestEnemy(GameObject exclude)
    {
        float nearest = Mathf.Infinity;
        float dist;
        EnemyStateManager target = null;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null) continue;
            if (enemies[i].gameObject == exclude) continue;

            dist = Vector3.Distance(transform.position, enemies[i].transform.position);

            if (dist < nearest)
            {
                nearest = dist;
                target = enemies[i];
            }
        }

        if (target == null) return null;
        return target.gameObject;
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
}
