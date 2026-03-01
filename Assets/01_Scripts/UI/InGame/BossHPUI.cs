using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    [SerializeField] private float hpPerBar = 200f;
    [SerializeField] private GameObject bossHpUI;
    [SerializeField] private Image nextHpFilled;
    [SerializeField] private Image currentHpFilled;
    [SerializeField] private Color[] barColors;

    private EnemyHealth boss;

    private void Start()
    {
        bossHpUI.SetActive(false);
    }

    private void Update()
    {
        UpdateBossHp();
    }

    private void OnValidate()
    {
        for (int i = 0; i < barColors.Length; i++)
        {
            barColors[i].a = 1f;
        }
    }

    public void ShowBossHp(EnemyHealth boss)
    {
        this.boss = boss;
        bossHpUI.SetActive(true);
    }

    public void HideBossHp()
    {
        bossHpUI.SetActive(false);
    }

    private void UpdateBossHp()
    {
        if (boss == null) return;

        if (boss.HP <= 0)
        {
            currentHpFilled.fillAmount = 0f;
            return;
        }

        int currentBarIndex = Mathf.CeilToInt(boss.HP / hpPerBar) - 1;

        float currentBarHp = boss.HP % hpPerBar;

        if (currentBarHp == 0)
        {
            currentBarHp = hpPerBar;
        }

        float fillAmount = currentBarHp / hpPerBar;

        currentHpFilled.fillAmount = fillAmount;
        currentHpFilled.color = barColors[currentBarIndex];

        if (currentBarIndex > 0)
        {
            nextHpFilled.color = barColors[currentBarIndex - 1];
        }
        else
        {
            nextHpFilled.color = Color.clear;
        }
    }
}