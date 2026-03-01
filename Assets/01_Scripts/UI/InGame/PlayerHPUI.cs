using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private Image playerHpFilled;
    private PlayerStateManager player;

    private void Update()
    {
        if (!player) return;
        UpdatePlayerHp();
    }

    private void UpdatePlayerHp()
    {
        playerHpFilled.fillAmount = player.health.HP / player.health.MaxHP;
    }

    public void SetPlayer(PlayerStateManager player)
    {
        this.player = player;
    }
}