using UnityEngine;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    [Header("※ Screen")]
    [SerializeField] private UIScreen lobbyScreen;
    [SerializeField] private UIScreen chapterScreen;
    [SerializeField] private UIScreen stageScreen;

    private readonly Stack<UIScreen> screenStack = new();
    private bool isTransitioning;

    protected override void Awake()
    {
        base.Awake();

        foreach (var screen in FindObjectsOfType<UIScreen>())
        {
            if (screen != lobbyScreen)
                screen.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (GameManager.Instance.ReturnToStage)
        {
            GameManager.Instance.SetReturnToStage(false);

            lobbyScreen.OnExit(null);
            stageScreen.OnEnter(null);

            screenStack.Push(chapterScreen);
            screenStack.Push(stageScreen);
        }
    }

    public void Open(UIScreen screen)
    {
        if (isTransitioning) return;

        UIScreen previous = null;
        bool hideLobby = false;

        if (screenStack.Count == 0 && screen.screenType == ScreenType.FullScreen)
            hideLobby = true;
        else if (screen.screenType == ScreenType.FullScreen)
            previous = screenStack.Peek();

        screenStack.Push(screen);
        isTransitioning = true;

        screen.OnEnter(() =>
        {
            if (hideLobby) lobbyScreen.OnExit(null);
            if (previous != null) previous.OnExit(null);
            isTransitioning = false;
        });
    }

    public void Back()
    {
        if (isTransitioning) return;
        if (screenStack.Count == 0) return;

        UIScreen current = screenStack.Pop();

        if (current.screenType == ScreenType.FullScreen && screenStack.Count > 0)
        {
            UIScreen previous = screenStack.Peek();
            isTransitioning = true;
            current.OnExit(() =>
            {
                previous.OnEnter(null);
                isTransitioning = false;
            });
        }
        else if (screenStack.Count == 0 && current.screenType == ScreenType.FullScreen)
        {
            isTransitioning = true;
            current.OnExit(() =>
            {
                lobbyScreen.OnEnter(null);
                isTransitioning = false;
            });
        }
        else if (screenStack.Count == 0)
        {
            current.OnExit(null);
        }
        else
        {
            current.OnExit(null);
        }
    }

    public void Home()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        FadeManager.Instance.PlayFade(FadeDirection.LeftToRight, () =>
        {
            while (screenStack.Count > 0)
            {
                screenStack.Pop().OnExit(null);
            }
            lobbyScreen.OnEnter(null);
            isTransitioning = false;
        }, 1f);
    }

    public void PopCurrent()
    {
        if (screenStack.Count > 0)
            screenStack.Pop();
    }
}
