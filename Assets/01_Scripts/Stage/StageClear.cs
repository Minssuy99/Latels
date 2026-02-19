using UnityEngine;
using DG.Tweening;
using System.Collections;

public class StageClear : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private CanvasGroup fadeImage;
    [SerializeField] private RectTransform fadeLeft;
    [SerializeField] private RectTransform fadeRight;
    [SerializeField] private RectTransform doorLeft;
    [SerializeField] private RectTransform doorRight;

    [SerializeField] private RectTransform clockImage;
    [SerializeField] private RectTransform clockLeft;
    [SerializeField] private RectTransform clockRight;
    [SerializeField] private float duration = 0.5f;

    [Header("클리어 UI")]
    [SerializeField] private VictoryUI victoryUI;

    private Transform startPoint;
    private Transform endPoint;

    public void SetCameraPoint(Transform cameraStartPoint, Transform cameraEndPoint)
    {
        startPoint = cameraStartPoint;
        endPoint =  cameraEndPoint;
    }

    private void Start()
    {
        StageManager.Instance.OnStageClear += StartClearCoroutine;

        float doorHeight = clockLeft.transform.parent.GetComponent<RectTransform>().rect.height;
        clockLeft.sizeDelta = new Vector2(doorHeight, 0f);
        clockRight.sizeDelta = new Vector2(doorHeight, 0f);

        clockLeft.anchoredPosition = Vector2.zero;
        clockRight.anchoredPosition = Vector2.zero;

        fadeImage.alpha = 0f;
        doorLeft.gameObject.SetActive(false);
        doorRight.gameObject.SetActive(false);
        clockLeft.gameObject.SetActive(false);
        clockRight.gameObject.SetActive(false);
        clockImage.gameObject.SetActive(false);
    }

    private void StartClearCoroutine()
    {
        StartCoroutine(ClearSequence());
    }

    IEnumerator ClearSequence()
    {
        TimeManager.Instance.StartHitStop();
        yield return new WaitForSecondsRealtime(1f);

        TimeManager.Instance.StopHitStop();
        yield return new WaitForSecondsRealtime(1f);

        inGameUI.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);

        StageManager.Instance.PlayerInput.enabled = false;
        yield return new WaitForSecondsRealtime(1f);
        fadeImage.DOFade(1f, 0.75f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1.5f);

        doorLeft.gameObject.SetActive(true);
        doorRight.gameObject.SetActive(true);
        clockImage.gameObject.SetActive(true);
        fadeLeft.DOAnchorPosX(-fadeLeft.rect.width, duration);
        fadeRight.DOAnchorPosX(fadeRight.rect.width, duration);

        yield return new WaitForSecondsRealtime(1f);
        clockImage.DORotate(new Vector3(0, 0, 360), 1.5f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            clockImage.gameObject.SetActive(false);
            clockLeft.gameObject.SetActive(true);
            clockRight.gameObject.SetActive(true);
        });
        yield return new WaitForSecondsRealtime(1.75f);
        Camera.main.GetComponent<FollowCamera>().enabled = false;
        Camera.main.transform.position = startPoint.position;
        Camera.main.transform.rotation = startPoint.rotation;

        doorLeft.DOAnchorPosX(-doorLeft.rect.width, duration);
        doorRight.DOAnchorPosX(doorRight.rect.width, duration);
        yield return new WaitForSecondsRealtime(2f);

        Camera.main.transform.DOMove(endPoint.position, 0.7f);
        Camera.main.transform.DORotate(endPoint.rotation.eulerAngles, 0.7f);
        yield return new WaitForSecondsRealtime(1.5f);

        victoryUI.Show("The Latels of the Cataclysm", 4635, 6000);
    }
}
