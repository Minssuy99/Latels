using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Character")]
    public CharacterData[] characterSlots = new CharacterData[3];

    [Header("Chapter/Stage")]
    public ChapterData chapterData;
    public StageData stageData;

    public Action onReturnToLobby;

    public void LoadGameScene(StageData stageData)
    {
        this.stageData = stageData;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}