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

    public void SelectChapter(ChapterData data) => chapterData = data;
    public void SelectStage(StageData data) => stageData = data;
    public void SetReturnToStage(bool value) => returnToStage = value;
    public void SetCharacterSlot(int index, CharacterData data) => characterSlots[index] = data;

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
        yield return new WaitForSecondsRealtime(0.5f);
        DOTween.KillAll();

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (op != null && !op.isDone)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        FadeManager.Instance.BlackFadeOut();
    }
}