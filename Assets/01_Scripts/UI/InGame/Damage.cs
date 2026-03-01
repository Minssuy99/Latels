using TMPro;
using UnityEngine;

public enum DamageType
{
    Enemy,
    Player,
    Critical,
}

public class Damage : MonoBehaviour
{
    [SerializeField] private float spawnRange = 50f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float fadeDelay = 0.2f;
    [SerializeField] private Vector3 punchScale = new Vector3(0.3f, 0.3f, 0);
    [SerializeField] private float punchDuration = 0.2f;

    [Header("Color Settings")]
    [SerializeField] private VertexGradient enemyGradient;
    [SerializeField] private VertexGradient playerGradient;
    [SerializeField] private VertexGradient criticalGradient;

    private TMP_Text damageText;
    private RectTransform rectTransform;
    private Transform target;
    private Vector2 screenOffset;
    private DamageType damageType;

    private float timer;
    private float punchTimer;
    private bool isPunching;
    private Vector3 originalScale;

    private void Awake()
    {
        damageText = GetComponentInChildren<TMP_Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetDamage(float damage, Transform targetTransform, Vector3 attackerPos, DamageType type)
    {
        this.target = targetTransform;
        this.damageType = type;
        damageText.text = ((int)damage).ToString();
        damageText.enableVertexGradient = true;
        damageText.colorGradient = GetGradient(type);
        damageText.alpha = 1;
        transform.localScale = Vector3.one;
        originalScale = Vector3.one;

        timer = 0;
        punchTimer = 0;
        isPunching = true;

        Vector3 headPos = target.position + Vector3.up;
        Vector3 enemyScreen = Camera.main.WorldToScreenPoint(headPos);

        if (type == DamageType.Player)
        {
            float offsetX = Random.Range(-spawnRange, spawnRange);
            float offsetY = Random.Range(-spawnRange, spawnRange);
            screenOffset = new Vector2(offsetX, offsetY);
        }
        else
        {
            Vector3 attackerScreen = Camera.main.WorldToScreenPoint(attackerPos);
            Vector2 dir = new Vector2(enemyScreen.x - attackerScreen.x, enemyScreen.y - attackerScreen.y).normalized;
            screenOffset = dir * spawnRange;
        }

        rectTransform.position = enemyScreen + new Vector3(screenOffset.x, screenOffset.y, 0);
    }

    private float GetDelta()
    {
        if (TimeManager.Instance.IsBulletTime)
            return TimeManager.Instance.PlayerDelta;
        return TimeManager.Instance.EnemyDelta;
    }

    private VertexGradient GetGradient(DamageType type)
    {
        return type switch
        {
            DamageType.Enemy => enemyGradient,
            DamageType.Player => playerGradient,
            DamageType.Critical => criticalGradient,
            _ => enemyGradient,
        };
    }

    private void Update()
    {
        if (target == null) return;

        float delta = GetDelta();
        timer += delta;

        if (isPunching)
        {
            punchTimer += delta;
            float t = punchTimer / punchDuration;
            if (t >= 1f)
            {
                isPunching = false;
                transform.localScale = originalScale;
            }
            else
            {
                float punch = Mathf.Sin(t * Mathf.PI) ;
                transform.localScale = originalScale + punchScale * punch;
            }
        }

        if (timer > fadeDelay)
        {
            float fadeT = (timer - fadeDelay) / fadeDuration;
            damageText.alpha = Mathf.Lerp(1, 0, fadeT);

            if (fadeT >= 1f)
            {
                PoolManager.Instance.Return(gameObject);
                return;
            }
        }

        Vector3 headPos = target.position + Vector3.up;
        Vector3 enemyScreen = Camera.main.WorldToScreenPoint(headPos);
        rectTransform.position = enemyScreen + new Vector3(screenOffset.x, screenOffset.y, 0);
    }
}
