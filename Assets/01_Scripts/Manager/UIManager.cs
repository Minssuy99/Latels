using UnityEngine;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    [Header("※ Screen")]
    [SerializeField] private UIScreen lobbyScreen;
    [SerializeField] private UIScreen chapterScreen;
    [SerializeField] private UIScreen stageScreen;

    private Stack<UIScreen> screenStack = new();
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
        if (GameManager.Instance.returnToStage)
        {
            GameManager.Instance.returnToStage = false;

            lobbyScreen.OnExit(null);
            chapterScreen.OnEnter(null);
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

        if (screenStack.Count == 0)
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
        else if (screenStack.Count == 0)
        {
            isTransitioning = true;
            current.OnExit(() =>
            {
                lobbyScreen.OnEnter(null);
                isTransitioning = false;
            });
        }
        else
        {
            current.OnExit(null);
        }
    }

    public void PopCurrent()
    {
        if (screenStack.Count > 0)
            screenStack.Pop();
    }
}
