using UnityEngine;

public class InGameUIManager : Singleton<InGameUIManager>
{
    [SerializeField] private VariableJoystick joystick;
    [SerializeField] private PlayerHPUI playerHpUI;
    [SerializeField] private DashUI dashUI;
    [SerializeField] private SkillUI skillUI;
    [SerializeField] private BossHPUI bossHpUI;
    [SerializeField] private VignetteUI vignetteUI;
    [SerializeField] private LockOnIndicatorUI lockOnIndicatorUI;
    [SerializeField] private DamagePopupUI damagePopupUI;

    public void SetPlayer(PlayerStateManager player)
    {
        player.move.SetJoystick(joystick);
        playerHpUI.SetPlayer(player);
        dashUI.SetPlayer(player);
        skillUI.SetPlayer(player);
        lockOnIndicatorUI.SetPlayer(player);

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.OnDamaged += (damage, attackerPos) =>
        {
            vignetteUI.ShowVignetteEffect();
            damagePopupUI.SpawnDamagePopup(damage, player.transform, attackerPos, DamageType.Player);
        };
    }

    public void SubscribeEnemy(EnemyHealth enemyHealth)
    {
        enemyHealth.OnDamaged += (damage, attackerPos) =>
        {
            damagePopupUI.SpawnDamagePopup(damage, enemyHealth.transform, attackerPos, DamageType.Enemy);
        };
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