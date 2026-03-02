using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 6f, -7f);
    private Transform player;
    private Vector3 originOffset;
    private float zoomRatio = 1f;

    private void Start()
    {
        originOffset = cameraOffset;
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    private void LateUpdate()
    {
        if (!player) return;

        transform.position = player.position + originOffset * zoomRatio;
    }

    public void ZoomIn(float targetRadio, float duration = 0, Ease ease = Ease.OutQuad)
    {
        StartCoroutine(ZoomSequence(targetRadio, duration, ease));
    }

    public void ZoomOut(float duration = 0f, Ease ease = Ease.OutQuad)
    {
        StartCoroutine(ZoomSequence(1f, duration, ease));
    }

    private IEnumerator ZoomSequence(float targetRatio, float duration, Ease ease = Ease.OutQuad)
    {
        DOTween.To(() => zoomRatio, x => zoomRatio = x, targetRatio, duration).SetUpdate(true).SetEase(ease);
        yield return new WaitForSeconds(duration);
    }
}