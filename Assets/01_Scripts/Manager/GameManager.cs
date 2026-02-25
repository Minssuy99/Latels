using UnityEngine;
using UnityEngine.SceneManagement;
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
        DOTween.KillAll();
        SceneManager.LoadScene("GameScene");
    }

    public void LoadLobbyScene()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("LobbyScene");
    }
}