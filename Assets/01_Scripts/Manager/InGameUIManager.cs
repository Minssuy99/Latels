using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIManager : Singleton<InGameUIManager>
{
    [Header("InGame Settings")]
    [SerializeField] private CanvasGroup damageVignette;
    [SerializeField] private float VignetteFadeOutSpeed = 0.5f;

    [Header("Player Settings")]
    private GameObject player;
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
    private PlayerAttack pa;
    private PlayerSkill ps;
    private PlayerDash pd;
    private SupportSkillManager sp;


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
    private EnemyAttack boss;

    private void Start()
    {
        mainSkill_Nbr.gameObject.SetActive(false);
        sub1_Skill_Nbr.gameObject.SetActive(false);
        dash_CanDashText.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdatePlayerHP();
        UpdateDashUI();
        UpdateCooldownUI(mainSkill_Bg, mainSkill_Img, mainSkill_Line, mainSkill_Nbr, ps.remainTime);
        UpdateCooldownUI(sub1_Skill_Bg, sub1_Skill_Img, sub1_Skill_Line, sub1_Skill_Nbr, sp.GetRemainTime(0));
        UpdateBossHP();

        if (damageVignette.alpha > 0)
        {
            damageVignette.alpha -= TimeManager.Instance.PlayerDelta * VignetteFadeOutSpeed;
        }
    }

    private void UpdatePlayerHP()
    {
        PlayerHP_Filled.fillAmount = pa.HP / pa.MaxHP;
    }

    private void UpdateDashUI()
    {
        dash_Count.text = pd.CurrentStack.ToString();
        dash_Filled.fillAmount = pd.ChargeFillAmount;

        if (pd.IsReuseDelay)
        {
            Color bg = dash_Bg.color;
            bg.a = 0.4f;
            dash_Bg.color = bg;

            Color ic = dash_Img.color;
            ic.a = 0.4f;
            dash_Img.color = ic;
            dash_CanDashText.gameObject.SetActive(true);
            dash_CanDashText.text = pd.ReuseTimer.ToString("F1");
        }
        else
        {
            Color bg = dash_Bg.color;
            bg.a = 0f;
            dash_Bg.color = bg;

            Color ic = dash_Img.color;
            ic.a = 1f;
            dash_Img.color = ic;
            dash_CanDashText.gameObject.SetActive(false);
        }
    }

    private void UpdateCooldownUI(Image background, Image icon, Image line, TextMeshProUGUI text, float remainTime)
    {
        if (remainTime > 0)
        {
            Color bg = background.color;
            bg.a = 0.4f;
            background.color = bg;

            Color ic = icon.color;
            ic.a = 0.4f;
            icon.color = ic;

            Color ln = line.color;
            ln.a = 0.4f;
            line.color = ln;

            text.gameObject.SetActive(true);
            text.text = Mathf.CeilToInt(remainTime).ToString();

            if (remainTime < 1)
            {
                text.text = remainTime.ToString("F1");
            }
        }
        else
        {
            Color bg = background.color;
            bg.a = 0f;
            background.color = bg;

            Color ic = icon.color;
            ic.a = 1f;
            icon.color = ic;

            Color ln = line.color;
            ln.a = 1f;
            line.color = ln;
            text.gameObject.SetActive(false);
        }
    }

    public void ShowBossHP(EnemyAttack boss)
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

    public void ShowDamageEffect()
    {
        damageVignette.alpha = 0.15f;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        pa = player.GetComponent<PlayerAttack>();
        ps = player.GetComponent<PlayerSkill>();
        pd = player.GetComponent<PlayerDash>();
        sp = player.GetComponent<SupportSkillManager>();
    }
}
