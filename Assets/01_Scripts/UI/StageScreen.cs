using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageScreen : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject stagePrefab;

    private List<GameObject> spawnedStages = new List<GameObject>();
    public void ShowStageScreen()
    {
        FadeManager.Instance.PlayFade(FadeDirection.RightToLeft, EnableStageScreen, 1);
    }

    public void HideStageScreen()
    {
        FadeManager.Instance.PlayFade(FadeDirection.LeftToRight, DisableStageScreen, 1);
    }

    private void EnableStageScreen()
    {
        gameObject.SetActive(true);
        gameObject.GetComponentInChildren<ScrollRect>().horizontalNormalizedPosition = 0;
        PlaceStagePrefab();
    }

    private void DisableStageScreen()
    {
        DestroyStagePrefab();
        gameObject.SetActive(false);
    }

    public void OpenDirect()
    {
        gameObject.SetActive(true);
        PlaceStagePrefab();
    }

    private void PlaceStagePrefab()
    {
        ChapterData data = GameManager.Instance.chapterData;

        for (int i = 0; i < data.stages.Length; i++)
        {
            GameObject stage = Instantiate(stagePrefab, content.transform);
            spawnedStages.Add(stage);

            RectTransform spawnRect = stage.GetComponent<RectTransform>();
            spawnRect.anchoredPosition = data.stages[i].stageScreenPosition;


            int index = i;
            stage.GetComponent<Button>().onClick.AddListener(() =>
            {
                LobbyManager.Instance.stageInfoPopup.ShowStageInfoPopup(index);
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