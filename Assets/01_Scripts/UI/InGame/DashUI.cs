using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    [SerializeField] private Image lineFilled;
    [SerializeField] private Image background;
    [SerializeField] private Image dashIcon;
    [SerializeField] private TMP_Text canDashText;
    [SerializeField] private TMP_Text dashCountText;

    private PlayerStateManager player;

    private void Start()
    {
        canDashText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!player) return;
        UpdateDashUI();
    }

    private void UpdateDashUI()
    {
        dashCountText.text = player.dash.CurrentStack.ToString();
        lineFilled.fillAmount = player.dash.ChargeFillAmount;

        if (player.dash.IsReuseDelay)
        {
            SetImageAlpha(background, 0.4f);
            SetImageAlpha(dashIcon, 0.4f);

            canDashText.gameObject.SetActive(true);
            canDashText.text = player.dash.ReuseTimer.ToString("F1");
        }
        else
        {
            SetImageAlpha(background, 0f);
            SetImageAlpha(dashIcon, 1f);

            canDashText.gameObject.SetActive(false);
        }
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color temp = image.color;
        temp.a = alpha;
        image.color = temp;
    }

    public void SetPlayer(PlayerStateManager player)
    {
        this.player = player;
    }
}