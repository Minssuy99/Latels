using UnityEngine;
using DG.Tweening;

public enum HideDirection
{
    Left,
    Right,
}

[System.Serializable]
public class UIElement
{
    public RectTransform rect;
    public HideDirection hideDirection;
}

public class LobbyManager : Singleton<LobbyManager>
{
    [Header("References")]
    public GameObject LobbyScreen;
    public ChapterScreen chapterScreen;
    public StageScreen stageScreen;
    public StageInfoPopup stageInfoPopup;
    public CharacterSelectScreen characterSelectScreen;

    [Header("Fader Settings")]
    [SerializeField] private UIElement[] elements;
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private float hideOffset;
    [SerializeField] private float duration;

    private bool isHidden = false;
    private bool isFullScreen = false;
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

        LobbyScreen.SetActive(true);
        chapterScreen.gameObject.SetActive(false);
        stageScreen.gameObject.SetActive(false);
        stageInfoPopup.gameObject.SetActive(false);
        backgroundPanel.SetActive(false);
        characterSelectScreen.gameObject.SetActive(false);

        GameManager.Instance.onReturnToLobby?.Invoke();
        GameManager.Instance.onReturnToLobby = null;
    }

    public void EnterFullScreen()
    {
        HideLobbyUI();
        isFullScreen = true;
        backgroundPanel.SetActive(true);
    }

    public void ExitFullScreen()
    {
        ShowLobbyUI();
        isFullScreen = false;
        backgroundPanel.SetActive(false);
    }

    public void HideLobbyUI()
    {
        if (isHidden) return;
        isHidden = true;

        float direction, targetX;

        for (int i = 0; i < elements.Length; i++)
        {
            direction = elements[i].hideDirection == HideDirection.Left ? -1 : 1;
            targetX = originalPositions[i].x + direction * hideOffset;

            elements[i].rect.DOAnchorPosX(targetX, duration).SetEase(Ease.InOutQuart) ;
            canvasGroups[i].DOFade(0f, duration).SetEase(Ease.InOutQuart) ;
        }
    }

    public void ShowLobbyUI()
    {
        if (!isHidden) return;
        isHidden = false;

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].rect.DOAnchorPos(originalPositions[i], duration).SetEase(Ease.InOutQuart) ;
            canvasGroups[i].DOFade(1f, duration).SetEase(Ease.InOutQuart) ;
        }
    }
}
