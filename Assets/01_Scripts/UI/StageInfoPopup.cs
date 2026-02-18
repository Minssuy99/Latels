using UnityEngine;

public class StageInfoPopup : MonoBehaviour
{
    public void ShowStageInfoPopup(int index)
    {
        GameManager.Instance.stageData = GameManager.Instance.chapterData.stages[index];
        gameObject.SetActive(true);
    }

    public void HideStageInfoPopup()
    {
        gameObject.SetActive(false);
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