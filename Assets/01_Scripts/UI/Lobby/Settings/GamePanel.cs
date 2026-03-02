using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePanel : MonoBehaviour
{
    [Header("Joystick")]
    [SerializeField] private Toggle joystickOnToggle;
    [SerializeField] private Toggle joystickOffToggle;

    [Header("FPS")]
    [SerializeField] private RectTransform fpsWarningAlert;
    [SerializeField] private Toggle fps60;
    [SerializeField] private Toggle fps30;
    [SerializeField] private Toggle fps120;

    private void OnEnable()
    {
        fpsWarningAlert.localScale = Vector3.one * 0.3f;
        fpsWarningAlert.gameObject.SetActive(false);
        SetJoystick();
        SetFPS();
    }

    public void OnJoystickOnToggle(bool isOn)
    {
        if (!isOn) return;
        SettingManager.Instance.JoystickVisible = true;
        SettingManager.Instance.Save();
    }

    public void OnJoystickOffToggle(bool isOn)
    {
        if (!isOn) return;
        SettingManager.Instance.JoystickVisible = false;
        SettingManager.Instance.Save();
    }

    public void OnFPS60Toggle(bool isOn)
    {
        if (!isOn) return;
        SettingManager.Instance.TargetFPS = 60;
        Application.targetFrameRate = 60;
        SettingManager.Instance.Save();
    }

    public void OnFPS30Toggle(bool isOn)
    {
        if (!isOn) return;
        SettingManager.Instance.TargetFPS = 30;
        Application.targetFrameRate = 30;
        SettingManager.Instance.Save();
    }

    public void OnFPS120Toggle(bool isOn)
    {
        if (!isOn) return;
        double refreshRate = Screen.currentResolution.refreshRateRatio.value;
        if (refreshRate < 300)
        {
            fpsWarningAlert.gameObject.SetActive(true);
            fpsWarningAlert.DOScale(1f, 0.3f);
            fpsWarningAlert.DOScale(0f, 0.15f).SetDelay(2f).OnComplete(() =>
            {
                fpsWarningAlert.gameObject.SetActive(false);
            });
        }
        SettingManager.Instance.TargetFPS = 120;
        Application.targetFrameRate = 120;
        SettingManager.Instance.Save();
    }

    private void SetJoystick()
    {
        joystickOnToggle.SetIsOnWithoutNotify(false);
        joystickOffToggle.SetIsOnWithoutNotify(false);

        if (SettingManager.Instance.JoystickVisible)
        {
            joystickOnToggle.SetIsOnWithoutNotify(true);
        }
        else
        {
            joystickOffToggle.SetIsOnWithoutNotify(true);
        }
    }

    private void SetFPS()
    {
        fps60.SetIsOnWithoutNotify(false);
        fps30.SetIsOnWithoutNotify(false);
        fps120.SetIsOnWithoutNotify(false);

        int fps = SettingManager.Instance.TargetFPS;
        switch (fps)
        {
            case 120:
                fps120.SetIsOnWithoutNotify(true);
                break;
            case 30:
                fps30.SetIsOnWithoutNotify(true);
                break;
            default:
                fps60.SetIsOnWithoutNotify(true);
                break;
        }
    }
}