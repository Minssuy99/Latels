using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerSkill : MonoBehaviour, ISkillComponent, ISupportSkill
{
    public float skillCoolTime => player.CharacterData.stats.skillCoolTime;
    public float remainTime;

    private bool canUseSkill = true;

    protected PlayerStateManager player;
    protected List<EnemyStateManager> enemies;

    private void Awake()
    {
        player = GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        if (canUseSkill == false)
        {
            remainTime -= TimeManager.Instance.PlayerDelta;

            if (remainTime <= 0)
                canUseSkill = true;
        }
    }

    protected GameObject FindNearestEnemy(GameObject exclude)
    {
        float nearest = Mathf.Infinity;
        float dist;
        EnemyStateManager target = null;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (!enemies[i]) continue;
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

    public void OnMainSkill(InputValue value)
    {
        if (player.IsUsingSkill) return;
        if (player.IsDead) return;
        if (player.targetEnemy == null) return;
        if (canUseSkill == false) return;

        canUseSkill = false;
        remainTime = skillCoolTime;
        player.ChangeState(player.skillState);
    }

    public void Initialize(List<EnemyStateManager> enemies)
    {
        this.enemies = enemies;
    }

    public virtual void OnSkillStart()
    {
        if(enemies == null || enemies.Count == 0)
            enemies = player.attack.GetEnemies();
        player.animator.SetTrigger("Skill");
    }
}