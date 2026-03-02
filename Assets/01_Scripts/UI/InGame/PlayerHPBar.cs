using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField] private Image trailFilled;
    [SerializeField] private Image filled;
    [SerializeField] private float headHeight;
    private PlayerStateManager player;
    private Camera cam;

    public void SetPlayer(PlayerStateManager player)
    {
        this.player = player;
        cam = Camera.main;
        filled.fillAmount = 1;
        trailFilled.fillAmount = 1;

        player.health.OnDamaged += OnDamaged;
        gameObject.SetActive(true);
    }

    private void OnDamaged(float damage, Vector3 attackPos)
    {
        filled.fillAmount = player.health.HP / player.health.MaxHP;
        trailFilled.DOKill();
        trailFilled.DOFillAmount(filled.fillAmount, 0.3f).SetDelay(0.3f);
    }

    private void Update()
    {
        if (!player) return;
        Vector2 screenPos = cam.WorldToScreenPoint(player.transform.position + Vector3.up * headHeight);
        transform.position = screenPos;
    }
}