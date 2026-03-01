using System;
using DG.Tweening;
using UnityEngine;

public class StageInfoPopup : UIScreen
{
    [Header("※ Popup")]
    [SerializeField] private RectTransform infoPanel;

    [Space(10)]
    [Header("※ Reference")]
    [SerializeField] private UIScreen characterSelectScreen;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public override void OnEnter(Action onComplete)
    {
        infoPanel.anchoredPosition = new Vector2(1000, 0);
        gameObject.SetActive(true);
        canvasGroup.DOFade(1, 0.25f);
        infoPanel.DOAnchorPosX(0, 0.25f).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public override void OnExit(Action onComplete)
    {
        canvasGroup.DOFade(0, 0.25f);
        infoPanel.DOAnchorPosX(1000, 0.25f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    public void GoToSelectScreen()
    {
        FadeManager.Instance.PlayFade(FadeDirection.RightToLeft, () =>
        {
            gameObject.SetActive(false);
            canvasGroup.alpha = 0;
            UIManager.Instance.PopCurrent();
            UIManager.Instance.Open(characterSelectScreen);
        }, 1);
    }
}
