using System;

public class MenuScreen : UIScreen
{
    public override void OnEnter(Action onComplete)
    {
        gameObject.SetActive(true);
        onComplete?.Invoke();
    }

    public override void OnExit(Action onComplete)
    {
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }
}