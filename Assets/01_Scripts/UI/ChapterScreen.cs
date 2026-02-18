using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterScreen : MonoBehaviour
{
    [SerializeField] private ChapterData[] chapters;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject chapterPrefab;

    private void Start()
    {
        PlaceChapterPrefab();
    }

    public void ShowChapterScreen()
    {
        FadeManager.Instance.PlayFade(FadeDirection.RightToLeft, EnableChapterScreen, 1);
    }

    public void HideChapterScreen()
    {
        FadeManager.Instance.PlayFade(FadeDirection.LeftToRight, DisableChapterScreen, 1);
    }

    private void EnableChapterScreen()
    {
        gameObject.SetActive(true);
    }

    private void DisableChapterScreen()
    {
        gameObject.SetActive(false);
    }

    private void PlaceChapterPrefab()
    {
        for (int i = 0; i < chapters.Length; i++)
        {
            GameObject chapter = Instantiate(chapterPrefab, content.transform);

            int index = i;
            chapter.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.Instance.chapterData = chapters[index];
                LobbyManager.Instance.stageScreen.ShowStageScreen();
            });
            chapter.GetComponentInChildren<TMP_Text>().text = $"Chapter {(int)chapters[i].chapterNumber + 1}";
        }
    }
}
