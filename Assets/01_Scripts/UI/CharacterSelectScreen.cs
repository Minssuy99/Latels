using System;
using TMPro;
using UnityEngine;

public class CharacterSelectScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text mainCharName;
    [SerializeField] private TMP_Text subChar1Name;
    [SerializeField] private TMP_Text subChar2Name;

    [SerializeField] private GameObject mainCharPos;
    [SerializeField] private GameObject subChar1Pos;
    [SerializeField] private GameObject subChar2Pos;

    private GameObject mainChar;
    private GameObject subChar1;
    private GameObject subChar2;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {
        float aspect = (float)Screen.width / Screen.height;
        cam.fieldOfView = Mathf.Lerp(60, 50, Mathf.InverseLerp(1.33f, 2.17f, aspect));

        if (mainChar == null)
        {
            mainChar = Instantiate(GameManager.Instance.mainCharData.displayPrefab, mainCharPos.transform);
            subChar1 = Instantiate(GameManager.Instance.supportChar1_Data.displayPrefab, subChar1Pos.transform);
            //subChar2 = Instantiate(GameManager.Instance.supportChar2_Data.displayPrefab, subChar2Pos.transform);

            mainCharName.text = GameManager.Instance.mainCharData.characterName;
            subChar1Name.text = GameManager.Instance.supportChar1_Data.characterName;
            //subChar2Name.text = GameManager.Instance.supportChar2_Data.characterName;
        }
    }

    public void HideSelectScreen()
    {
        FadeManager.Instance.PlayFade(FadeDirection.LeftToRight, DisableSelectScreen, 1);
    }

    public void StartGame()
    {
        GameManager.Instance.onReturnToLobby = () =>
        {
            LobbyManager.Instance.LobbyScreen.SetActive(true);
            LobbyManager.Instance.chapterScreen.gameObject.SetActive(true);
            LobbyManager.Instance.stageScreen.OpenDirect();
        };
        FadeManager.Instance.PlayFade(FadeDirection.RightToLeft, () =>
        {
            GameManager.Instance.LoadGameScene(GameManager.Instance.stageData);
        }, 1);
    }

    private void DisableSelectScreen()
    {
        LobbyManager.Instance.LobbyScreen.SetActive(true);
        LobbyManager.Instance.chapterScreen.gameObject.SetActive(true);
        LobbyManager.Instance.stageScreen.OpenDirect();
        LobbyManager.Instance.characterSelectScreen.gameObject.SetActive(false);
    }
}