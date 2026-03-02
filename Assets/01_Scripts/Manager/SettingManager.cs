using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    public bool JoystickVisible { get; set; }
    public int TargetFPS { get; set; }

    protected override void Awake()
    {
        base.Awake();
        JoystickVisible = LoadBool("JoystickVisible", true);

        QualitySettings.vSyncCount = 0;
        TargetFPS = LoadInt("TargetFPS", 60);
    }

    public void Save()
    {
        SaveBool("JoystickVisible", JoystickVisible);
        SaveInt("TargetFPS", TargetFPS);
        PlayerPrefs.Save();
    }

    private void SaveInt(string key, int value) => PlayerPrefs.SetInt(key, value);
    private void SaveFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
    private void SaveBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);

    private int LoadInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
    private float LoadFloat(string key, float defaultValue) => PlayerPrefs.GetFloat(key, defaultValue);
    private bool LoadBool(string key, bool defaultValue) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
}