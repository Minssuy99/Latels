using System.Collections.Generic;

public interface ISupportSkill
{
    void Initialize(List<EnemyStateManager> enemies);
    void OnSkillStart();
}
