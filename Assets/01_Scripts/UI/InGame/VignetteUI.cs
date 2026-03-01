using UnityEngine;

public class VignetteUI : MonoBehaviour
{
    [SerializeField] private float vignetteFadeOutSpeed = 0.5f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= TimeManager.Instance.PlayerDelta * vignetteFadeOutSpeed;
        }
    }

    public void ShowVignetteEffect()
    {
        canvasGroup.alpha = 0.15f;
    }
}