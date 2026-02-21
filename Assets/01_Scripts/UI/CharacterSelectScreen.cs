using System;
using TMPro;
using UnityEngine;

public class CharacterSelectScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text[] characterName;
    [SerializeField] private GameObject[] characterPosition;

    private GameObject[] character = new GameObject[3];
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {
        float aspect = (float)Screen.width / Screen.height;
        cam.fieldOfView = Mathf.Lerp(60, 50, Mathf.InverseLerp(1.33f, 2.17f, aspect));

        for (int i = 0; i < GameManager.Instance.characterSlots.Length; i++)
        {
            if (GameManager.Instance.characterSlots[i] == null) continue;

            if (character[i] == null)
            {
                characterName[i].text = GameManager.Instance.characterSlots[i].charName;
                character[i] = Instantiate(GameManager.Instance.characterSlots[i].Prefab, characterPosition[i].transform);
                character[i].GetComponent<CharacterSetup>().SetRole(CharacterRole.Display);
            }
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