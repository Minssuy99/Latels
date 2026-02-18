using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Prefabs")]
    public CharacterData mainCharData;
    public CharacterData supportChar1_Data;
    public CharacterData supportChar2_Data;

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