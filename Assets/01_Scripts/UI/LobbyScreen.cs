using System;
using UnityEngine;
using DG.Tweening;

public enum HideDirection
{
    Left,
    Right,
}

[Serializable]
public class UIElement
{
    public RectTransform rect;
    public HideDirection hideDirection;
}

public class LobbyScreen : UIScreen
{
    [Header("※ UI Animation")]
    [SerializeField] private UIElement[] elements;
    [SerializeField] private float hideOffset;
    [SerializeField] private float duration;

    [Space(10)]
    [Header("※ Background")]
    [SerializeField] private GameObject backgroundPanel;

    private bool isHidden;
    private Vector2[] originalPositions;
    private CanvasGroup[] canvasGroups;

    private void Start()
    {
        originalPositions = new Vector2[elements.Length];
        canvasGroups = new CanvasGroup[elements.Length];

        for (int i = 0; i < elements.Length; i++)
        {
            originalPositions[i] = elements[i].rect.anchoredPosition;
            canvasGroups[i] = elements[i].rect.GetComponent<CanvasGroup>();
        }

        backgroundPanel.SetActive(false);
    }

    public override void OnEnter(Action onComplete)
    {
        gameObject.SetActive(true);
        ShowLobbyUI();
        onComplete?.Invoke();
    }

    public override void OnExit(Action onComplete)
    {
        HideLobbyUI();
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    public void EnterFullScreen()
    {
        HideLobbyUI();
        backgroundPanel.SetActive(true);
    }

    public void ExitFullScreen()
    {
        ShowLobbyUI();
        backgroundPanel.SetActive(false);
    }

    private void HideLobbyUI()
    {
        if (isHidden) return;
        isHidden = true;

        float direction, targetX;

        for (int i = 0; i < elements.Length; i++)
        {
            direction = elements[i].hideDirection == HideDirection.Left ? -1 : 1;
            targetX = originalPositions[i].x + direction * hideOffset;

            elements[i].rect.DOAnchorPosX(targetX, duration).SetEase(Ease.InOutQuart);
            canvasGroups[i].DOFade(0f, duration).SetEase(Ease.InOutQuart);
        }
    }

    private void ShowLobbyUI()
    {
        if (!isHidden) return;
        isHidden = false;

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].rect.DOAnchorPos(originalPositions[i], duration).SetEase(Ease.InOutQuart);
            canvasGroups[i].DOFade(1f, duration).SetEase(Ease.InOutQuart);
        }
    }
}