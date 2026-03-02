using System;
using UnityEngine;

public enum ScreenType
{
    FullScreen,
    Popup,
}

public class UIScreen : MonoBehaviour
{
    [Header("※ Screen Type")]
    public ScreenType screenType;

    public void Open()
    {
        UIManager.Instance.Open(this);
    }

    public void Close()
    {
        UIManager.Instance.Back();
    }

    public void GoHome()
    {
        UIManager.Instance.Home();
    }

    public virtual void OnEnter(Action onComplete)
    {

    }

    public virtual void OnExit(Action onComplete)
    {

    }
}
