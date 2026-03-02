using UnityEngine;

public class InGameUIManager : Singleton<InGameUIManager>
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private PlayerHpBar playerHpBar;
    [SerializeField] private DashUI dashUI;
    [SerializeField] private SkillUI skillUI;
    [SerializeField] private BossHPUI bossHpUI;
    [SerializeField] private VignetteUI vignetteUI;
    [SerializeField] private LockOnIndicatorUI lockOnIndicatorUI;
    [SerializeField] private DamageHolder damageHolder;
    [SerializeField] private EnemyHpHolder enemyHpHolder;

    public void SetPlayer(PlayerStateManager player)
    {
        player.move.SetJoystick(joystick);
        playerHpBar.SetPlayer(player);
        dashUI.SetPlayer(player);
        skillUI.SetPlayer(player);
        lockOnIndicatorUI.SetPlayer(player);

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.OnDamaged += (damage, attackerPos) =>
        {
            vignetteUI.ShowVignetteEffect();
            damageHolder.SpawnDamagePopup(damage, player.transform, attackerPos, DamageType.Player);
        };
    }

    public void SubscribeEnemy(EnemyHealth enemyHealth)
    {
        enemyHpHolder.CreateHpBar(enemyHealth);
        enemyHealth.OnDamaged += (damage, attackerPos) =>
        {
            damageHolder.SpawnDamagePopup(damage, enemyHealth.transform, attackerPos, DamageType.Enemy);
        };
    }

    public void CreateEnemyHpBar(EnemyHealth enemy)
    {
        enemyHpHolder.CreateHpBar(enemy);
    }

    public void ShowBossHp(EnemyHealth boss)
    {
        bossHpUI.ShowBossHp(boss);
    }

    public void HideBossHp()
    {
        bossHpUI.HideBossHp();
    }
}