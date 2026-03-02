using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject ExitPopUp;
    [SerializeField] private SettingsScreen settings;

    private void Awake()
    {
        menuPanel.SetActive(false);
        ExitPopUp.SetActive(false);
        settings.gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        TimeManager.Instance.Pause();
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
        TimeManager.Instance.Resume();
    }

    public void OpenSettings()
    {
        settings.OnEnter(null);
    }

    public void CloseSettings()
    {
        settings.OnExit(null);
    }

    public void OpenExitPopUp()
    {
        ExitPopUp.SetActive(true);
    }

    public void CloseExitPopUp()
    {
        ExitPopUp.SetActive(false);
    }

    public void ExitToLobby()
    {
        TimeManager.Instance.Resume();
        GameManager.Instance.LoadLobbyScene();
    }
}