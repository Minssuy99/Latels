using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public CharacterData[] CharacterSlots { get; private set; } = new CharacterData[3];
    public ChapterData ChapterData { get; private set; }
    public StageData StageData { get; private set; }
    public bool ReturnToStage { get; private set; }

    public void SetCharacterSlot(int index, CharacterData data) => CharacterSlots[index] = data;
    public void SelectChapter(ChapterData data) => ChapterData = data;
    public void SelectStage(StageData data) => StageData = data;
    public void SetReturnToStage(bool value) => ReturnToStage = value;

    public void LoadGameScene(StageData stageData)
    {
        StageData = stageData;
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