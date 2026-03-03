using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] private Image trailFilled;
    [SerializeField] private Image filled;
    [SerializeField] private float headHeight;
    private Camera cam;
    private Transform target;
    private EnemyHealth enemyHealth;

    public void SetTarget(EnemyHealth enemy)
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDamaged -= OnDamaged;
        }

        cam = Camera.main;
        enemyHealth = enemy;
        target = enemy.transform;

        filled.fillAmount = 1;
        trailFilled.fillAmount = 1;

        enemy.OnDamaged += OnDamaged;
        gameObject.SetActive(false);
    }

    private void OnDamaged(float damage, Vector3 attackPos)
    {
        gameObject.SetActive(true);
        filled.fillAmount = enemyHealth.HP / enemyHealth.MaxHP;
        trailFilled.DOKill();
        trailFilled.DOFillAmount(filled.fillAmount, 0.3f).SetDelay(0.3f);

        if (enemyHealth.HP <= 0)
        {
            enemyHealth.OnDamaged -= OnDamaged;
            PoolManager.Instance.Return(gameObject);
        }
    }

    private void Update()
    {
        if (!target) return;
        Vector2 screenPos = cam.WorldToScreenPoint(target.position + Vector3.up * headHeight);
        transform.position = screenPos;
    }
}