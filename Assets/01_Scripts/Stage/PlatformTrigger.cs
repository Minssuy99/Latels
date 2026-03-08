using DG.Tweening;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private Transform platform;
    [SerializeField] private Vector3 platformEndPos;
    [SerializeField] private float platformMoveDuration;
    [SerializeField] private Ease platformEase = Ease.InOutSine;

    [Header("Gate Settings")]
    [SerializeField] private GameObject transparentWall;
    [SerializeField] private Transform entranceGate;
    [SerializeField] private Vector3 entranceEndPos;
    [SerializeField] private Transform exitGate;
    [SerializeField] private Vector3 exitEndPos;
    [SerializeField] private Ease gateEase = Ease.InOutSine;
    [SerializeField] private float doorMoveDuration = 2f;
    [SerializeField] private float triggerDelay = 1f;

    private bool isTriggered;

    private void Start()
    {
        transparentWall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;
        if (!other.CompareTag(GameTags.Player)) return;

        isTriggered = true;

        transparentWall.SetActive(true);

        entranceGate.DOLocalMove(entranceEndPos, doorMoveDuration).SetEase(gateEase).SetDelay(triggerDelay)
            .OnComplete(() =>
            {
                platform.DOMove(platformEndPos, platformMoveDuration).SetEase(platformEase)
                    .OnComplete(() =>
                    {
                        exitGate.DOLocalMove(exitEndPos, doorMoveDuration).SetEase(gateEase).SetDelay(triggerDelay);
                    });
            });
    }
}