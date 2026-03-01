using UnityEngine;
using DG.Tweening;

public class LockOnIndicatorUI : MonoBehaviour
{
    [SerializeField] private RectTransform lockOnIndicator;
    private Transform lastLockOnTarget;
    private PlayerStateManager player;
    private Camera cam;
    private Collider targetCollider;

    private void Start()
    {
        cam = Camera.main;
        lockOnIndicator.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!player) return;
        UpdateLockOnIndicator();
    }

    private void UpdateLockOnIndicator()
    {
        if (player.isLockedOn && player.targetEnemy)
        {
            lockOnIndicator.gameObject.SetActive(true);

            bool isNewTarget = player.targetEnemy.transform != lastLockOnTarget;
            if (isNewTarget)
            {
                targetCollider = player.targetEnemy.GetComponent<Collider>();
            }

            Vector3 screenPos = cam.WorldToScreenPoint(targetCollider.bounds.center);

            if (!isNewTarget)
            {
                lockOnIndicator.position = screenPos;
            }
            else if (!lastLockOnTarget)
            {
                lockOnIndicator.position = screenPos;
                lastLockOnTarget = player.targetEnemy.transform;
                lockOnIndicator.localScale = Vector3.one * 3f;
                lockOnIndicator.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic).SetUpdate(true);
            }
            else
            {
                lockOnIndicator.position = Vector3.Lerp(lockOnIndicator.position, screenPos, Time.unscaledDeltaTime * 50f);
                if (Vector3.Distance(lockOnIndicator.position, screenPos) < 1f)
                    lastLockOnTarget = player.targetEnemy.transform;
            }
        }
        else
        {
            lockOnIndicator.gameObject.SetActive(false);
            lastLockOnTarget = null;
        }
    }

    public void SetPlayer(PlayerStateManager player)
    {
        this.player = player;
    }
}