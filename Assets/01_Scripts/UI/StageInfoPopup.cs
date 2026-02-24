using DG.Tweening;
using UnityEngine;

public class StageInfoPopup : MonoBehaviour
{
    [SerializeField] private RectTransform infoPanel;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        infoPanel = infoPanel.GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void ShowStageInfoPopup(int index)
    {
        infoPanel.anchoredPosition = new Vector2(1000, 0);
        GameManager.Instance.stageData = GameManager.Instance.chapterData.stages[index];
        gameObject.SetActive(true);
        infoPanel.DOAnchorPosX(0, 0.25f);
        canvasGroup.DOFade(1, 0.25f);
    }

    public void HideStageInfoPopup()
    {
        canvasGroup.DOFade(0, 0.25f);
        infoPanel.DOAnchorPosX(1000, 0.25f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void GoToSelectScreen()
    {
        FadeManager.Instance.PlayFade(FadeDirection.RightToLeft, SelectScreen, 1);
    }

    private void SelectScreen()
    {
        LobbyManager.Instance.LobbyScreen.SetActive(false);
        LobbyManager.Instance.chapterScreen.gameObject.SetActive(false);
        LobbyManager.Instance.stageScreen.gameObject.SetActive(false);
        LobbyManager.Instance.stageInfoPopup.gameObject.SetActive(false);
        LobbyManager.Instance.characterSelectScreen.gameObject.SetActive(true);
    }
}