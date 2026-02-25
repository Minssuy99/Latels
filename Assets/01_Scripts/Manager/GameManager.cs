using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [Header("Character")]
    public CharacterData[] characterSlots = new CharacterData[3];

    [Header("Chapter/Stage")]
    public ChapterData chapterData;
    public StageData stageData;

    public bool returnToStage;

    public void LoadGameScene(StageData stageData)
    {
        this.stageData = stageData;
        StartCoroutine(LoadSceneCoroutine("GameScene"));
    }

    public void LoadLobbyScene()
    {
        StartCoroutine(LoadSceneCoroutine("LobbyScene"));
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        FadeManager.Instance.BlackFadeIn();
        yield return new WaitForSecondsRealtime(1.5f);
        DOTween.KillAll();

        FadeManager.Instance.ShowProgressBar();
        yield return new WaitForSecondsRealtime(1f);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            FadeManager.Instance.SetProgress(op.progress / 0.9f);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        FadeManager.Instance.HideProgressBar();
        yield return new WaitForSecondsRealtime(1.5f);
        FadeManager.Instance.BlackFadeOut();
    }
}