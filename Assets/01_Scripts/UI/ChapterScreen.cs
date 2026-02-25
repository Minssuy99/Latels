using System;
using UnityEngine;
using UnityEngine.UI;

public class ChapterScreen : UIScreen
{
    [Header("※ Chapter")]
    [SerializeField] private ChapterData[] chapters;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject chapterPrefab;

    [Space(10)]
    [Header("※ Reference")]
    [SerializeField] private UIScreen stageScreen;

    private void Start()
    {
        PlaceChapterPrefab();
    }

    public override void OnEnter(Action onComplete)
    {
        if (onComplete != null)
        {
            FadeManager.Instance.PlayFade(FadeDirection.RightToLeft, () =>
            {
                onComplete.Invoke();
                gameObject.SetActive(true);
            }, 1);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public override void OnExit(Action onComplete)
    {
        if (onComplete != null)
        {
            FadeManager.Instance.PlayFade(FadeDirection.LeftToRight, () =>
            {
                gameObject.SetActive(false);
                onComplete.Invoke();
            }, 1);
        }
        else
        {
            gameObject.SetActive(false);
        }
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
                UIManager.Instance.Open(stageScreen);
            });
            chapter.GetComponent<ChapterItem>().Setup(chapters[i]);
        }
    }
}
