using System;
using UnityEngine;

enum Panels
{
    GamePanel,
    SoundPanel,
    LanguagePanel,
}

public class SettingsScreen : UIScreen
{
    [SerializeField] private GameObject[] panels;

    public void SelectTab(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        panels[index].SetActive(true);
    }

    public override void OnEnter(Action onComplete)
    {
        gameObject.SetActive(true);
        SelectTab((int)Panels.GamePanel);
        onComplete?.Invoke();
    }

    public override void OnExit(Action onComplete)
    {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }
}