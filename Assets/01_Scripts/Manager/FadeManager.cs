using System;
using UnityEngine;
using DG.Tweening;

public enum FadeDirection
{
    RightToLeft,
    LeftToRight
}

public class FadeManager : Singleton<FadeManager>
{
    [SerializeField] private GameObject fader;
    [SerializeField] private float halftoneOffset;
    [SerializeField] private float duration;
    public float Duration => duration;

    [SerializeField] private RectTransform clockImage;
    [SerializeField] private float clockSpeed;

    private RectTransform faderRect;
    private CanvasGroup canvasGroup;
    private float screenWidth;
    private bool isPlaying;

    private float clockRotation;
    private Tween clockTween;

    private void Start()
    {
        screenWidth = Screen.width;
        canvasGroup = fader.GetComponent<CanvasGroup>();
        faderRect = fader.GetComponent<RectTransform>();
        faderRect.anchoredPosition = new Vector2(screenWidth + halftoneOffset, 0);
        canvasGroup.alpha = 1;
        fader.SetActive(false);
    }

    public void PlayFade(FadeDirection direction,Action onScreenCovered, float holdTime)
    {
        if (isPlaying) return;
        isPlaying = true;

        fader.SetActive(true);

        float rotateDir = (direction == FadeDirection.RightToLeft) ? 360f : -360f;
        float Speed = (direction == FadeDirection.RightToLeft) ? clockSpeed : clockSpeed / 3f;
        float totalOffset = screenWidth + halftoneOffset;
        float startX, endX;

        if (direction == FadeDirection.RightToLeft)
        {
            startX = totalOffset;
            endX = -totalOffset;
        }
        else
        {
            startX = -totalOffset;
            endX = totalOffset;
        }

        faderRect.anchoredPosition = new Vector2(startX, 0);
        canvasGroup.alpha = 1f;

        faderRect.DOAnchorPosX(0, duration).SetEase(Ease.InOutQuint).OnComplete(() =>
        {
            onScreenCovered?.Invoke();

            clockTween = clockImage.DORotate(new Vector3(0, 0, rotateDir), Speed, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1)
                .SetRelative(true);

            DOVirtual.DelayedCall(holdTime - 0.25f, () =>
            {
                clockTween?.Kill();
                DOVirtual.DelayedCall(0.25f, () =>
                {
                    faderRect.DOAnchorPosX(endX, duration).SetEase(Ease.InOutQuint).OnComplete(() =>
                    {
                        isPlaying = false;
                        fader.SetActive(false);
                    });
                });
            });
        });
    }
}
