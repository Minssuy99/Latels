public class RuruneAttack : PlayerAttack, IBattleComponent
{
    public void Shoot()
    {
        if (player.targetEnemy == null) return;
        player.targetEnemy.attack.TakeDamage(player.CharacterData.stats.damage);
    }
}