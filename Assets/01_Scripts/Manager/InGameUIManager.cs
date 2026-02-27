using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIManager : Singleton<InGameUIManager>
{
    [Header("InGame Settings")]
    public Transform damageHolder;
    [SerializeField] private CanvasGroup damageVignette;
    [SerializeField] private float VignetteFadeOutSpeed = 0.5f;

    [Header("Player Settings")]
    [SerializeField] private Image PlayerHP_Filled;

    [Header("Dash Settings")]
    [SerializeField] private Image dash_Filled;
    [SerializeField] private Image dash_Bg;
    [SerializeField] private Image dash_Img;
    [SerializeField] private TextMeshProUGUI dash_CanDashText;
    [SerializeField] private TextMeshProUGUI dash_Count;

    [Header("Player Skill Settings")]
    [SerializeField] private Image mainSkill_Bg;
    [SerializeField] private Image mainSkill_Img;
    [SerializeField] private Image mainSkill_Line;
    [SerializeField] private TextMeshProUGUI mainSkill_Nbr;
    private PlayerStateManager player;


    [Header("Sub Character 1 Skill Settings")]
    [SerializeField] private Image sub1_Skill_Bg;
    [SerializeField] private Image sub1_Skill_Img;
    [SerializeField] private Image sub1_Skill_Line;
    [SerializeField] private TextMeshProUGUI sub1_Skill_Nbr;


    [Header("Boss Settings")]
    [SerializeField] private GameObject BossHpUI;
    [SerializeField] private Image nextBar;
    [SerializeField] private Image currentBar;
    [SerializeField] private Color[] barColors;
    [SerializeField] private float hpPerBar = 200f;
    private EnemyHealth boss;

    private void Start()
    {
        mainSkill_Nbr.gameObject.SetActive(false);
        sub1_Skill_Nbr.gameObject.SetActive(false);
        dash_CanDashText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        UpdatePlayerHP();
        UpdateDashUI();
        UpdateCooldownUI(mainSkill_Bg, mainSkill_Img, mainSkill_Line, mainSkill_Nbr, player.skill.remainTime);
        UpdateCooldownUI(sub1_Skill_Bg, sub1_Skill_Img, sub1_Skill_Line, sub1_Skill_Nbr, player.supportSkill.GetRemainTime(0));
        UpdateBossHP();

        if (damageVignette.alpha > 0)
        {
            damageVignette.alpha -= TimeManager.Instance.PlayerDelta * VignetteFadeOutSpeed;
        }
    }

    private void UpdatePlayerHP()
    {
        PlayerHP_Filled.fillAmount = player.health.HP / player.health.MaxHP;
    }

    private void UpdateDashUI()
    {
        dash_Count.text = player.dash.CurrentStack.ToString();
        dash_Filled.fillAmount = player.dash.ChargeFillAmount;

        if (player.dash.IsReuseDelay)
        {
            SetImageAlpha(dash_Bg, 0.4f);
            SetImageAlpha(dash_Img, 0.4f);

            dash_CanDashText.gameObject.SetActive(true);
            dash_CanDashText.text = player.dash.ReuseTimer.ToString("F1");
        }
        else
        {
            SetImageAlpha(dash_Bg, 0f);
            SetImageAlpha(dash_Img, 1f);

            dash_CanDashText.gameObject.SetActive(false);
        }
    }

    private void UpdateCooldownUI(Image background, Image icon, Image line, TextMeshProUGUI text, float remainTime)
    {
        if (remainTime > 0)
        {
            SetImageAlpha(background, 0.4f);
            SetImageAlpha(icon, 0.4f);
            SetImageAlpha(line, 0.4f);

            text.gameObject.SetActive(true);
            text.text = Mathf.CeilToInt(remainTime).ToString();

            if (remainTime < 1)
            {
                text.text = remainTime.ToString("F1");
            }
        }
        else
        {
            SetImageAlpha(background, 0f);
            SetImageAlpha(icon, 1f);
            SetImageAlpha(line, 1f);


            text.gameObject.SetActive(false);
        }
    }

    public void ShowBossHP(EnemyHealth boss)
    {
        this.boss = boss;
        BossHpUI.SetActive(true);
    }

    public void HideBossHP()
    {
        BossHpUI.SetActive(false);
    }

    private void UpdateBossHP()
    {
        if (boss == null) return;

        if (boss.HP <= 0)
        {
            currentBar.fillAmount = 0f;
            return;
        }

        int currentBarIndex = Mathf.CeilToInt(boss.HP / hpPerBar) - 1;

        float currentBarHP = boss.HP % hpPerBar;

        if (currentBarHP == 0)
        {
            currentBarHP = hpPerBar;
        }

        float fillAmount = currentBarHP / hpPerBar;

        currentBar.fillAmount = fillAmount;
        currentBar.color = barColors[currentBarIndex];

        if (currentBarIndex > 0)
        {
            nextBar.color = barColors[currentBarIndex - 1];
        }
        else
        {
            nextBar.color = Color.clear;
        }
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color temp = image.color;
        temp.a = alpha;
        image.color = temp;
    }

    public void ShowDamageEffect()
    {
        damageVignette.alpha = 0.15f;
    }

    public void SetPlayer(PlayerStateManager player)
    {
        this.player = player;
    }
}
