using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageScreen : UIScreen
{
    [Header("※ Stage")]
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject stagePrefab;

    [Space(10)]
    [Header("※ Reference")]
    [SerializeField] private UIScreen stageInfoPopup;

    private ScrollRect scrollRect;
    private List<GameObject> spawnedStages = new ();

    private void Awake()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
    }

    public override void OnEnter(Action onComplete)
    {
        if (onComplete != null)
        {
            FadeManager.Instance.PlayFade(FadeDirection.RightToLeft, () =>
            {
                onComplete.Invoke();
                EnterScreen();
            }, 1);
        }
        else
        {
            EnterScreen();
        }
    }

    public override void OnExit(Action onComplete)
    {
        if (onComplete != null)
        {
            FadeManager.Instance.PlayFade(FadeDirection.LeftToRight, () =>
            {
                ExitScreen();
                onComplete.Invoke();
            }, 1);
        }
        else
        {
            ExitScreen();
        }
    }

    private void EnterScreen()
    {
        gameObject.SetActive(true);
        scrollRect.horizontalNormalizedPosition = 0;
        PlaceStagePrefab();
    }

    private void ExitScreen()
    {
        DestroyStagePrefab();
        gameObject.SetActive(false);
    }

    private void PlaceStagePrefab()
    {
        ChapterData data = GameManager.Instance.chapterData;

        for (int i = 0; i < data.stages.Length; i++)
        {
            GameObject stage = Instantiate(stagePrefab, content.transform);
            spawnedStages.Add(stage);

            stage.GetComponent<RectTransform>().anchoredPosition = data.stages[i].stageScreenPosition;

            int index = i;
            stage.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.Instance.SelectStage(GameManager.Instance.chapterData.stages[index]);
                UIManager.Instance.Open(stageInfoPopup);
            });

            string chapterName = ((int)data.chapterNumber + 1).ToString();
            string stageName = data.stages[i].stageNumber.ToString();

            stage.GetComponentInChildren<TMP_Text>().text = $"{chapterName} - {stageName}";
        }
    }

    private void DestroyStagePrefab()
    {
        foreach (var stage in spawnedStages)
        {
            Destroy(stage);
        }
        spawnedStages.Clear();
    }
}