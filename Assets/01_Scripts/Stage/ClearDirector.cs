using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ClearDirector : MonoBehaviour
{
    [Header("※ Fade")]
    [SerializeField] private CanvasGroup fadeImage;
    [SerializeField] private RectTransform fadeLeft;
    [SerializeField] private RectTransform fadeRight;

    [Space(10)]
    [Header("※ Clock")]
    [SerializeField] private RectTransform clockImage;
    [SerializeField] private RectTransform clockLeft;
    [SerializeField] private RectTransform clockRight;

    [Space(10)]
    [Header("※ Door")]
    [SerializeField] private RectTransform doorLeft;
    [SerializeField] private RectTransform doorRight;

    [Space(10)]
    [Header("※ Result Screen")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform victoryRect;
    [SerializeField] private CanvasGroup stageTitleGroup;
    [SerializeField] private TMP_Text stageTitleText;
    [SerializeField] private RectTransform exp;
    [SerializeField] private Image expFill;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private CanvasGroup endPanel;

    [Space(10)]
    [Header("※ Reference")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private float duration = 0.5f;

    private List<Animator> displayCharacters = new ();
    private Transform startPoint;
    private Transform endPoint;
    private Camera cam;

    private TMP_Text victoryText;
    private CanvasGroup expGroup;
    private float originalY;

    private void Awake()
    {
        cam = Camera.main;
        expGroup = exp.GetComponent<CanvasGroup>();
        victoryText = victoryRect.GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StageManager.Instance.OnStageClear += () => StartCoroutine(ClearSequence());

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

        endPanel.gameObject.SetActive(false);
        endPanel.alpha = 0f;
        panel.gameObject.SetActive(false);

        Button continueButton = endPanel.GetComponent<Button>();
        continueButton.onClick.AddListener(() =>
        {
            InGameUIManager.Instance.enabled = false;
            GameManager.Instance.LoadLobbyScene();
        });
    }

    public void SetCameraPoint(Transform cameraStartPoint, Transform cameraEndPoint)
    {
        startPoint = cameraStartPoint;
        endPoint = cameraEndPoint;
    }

    public void SetDisplayAnimators(List<Animator> displayCharacters)
    {
        this.displayCharacters = displayCharacters;
    }

    private IEnumerator ClearSequence()
    {
        yield return HitStopPhase();
        yield return FadePhase();
        yield return DoorPhase();
        yield return CameraPhase();
        yield return ResultUIPhase();
    }

    private IEnumerator HitStopPhase()
    {
        TimeManager.Instance.StartHitStop();
        yield return new WaitForSecondsRealtime(1f);

        TimeManager.Instance.StopHitStop();
        yield return new WaitForSecondsRealtime(1f);
    }

    private IEnumerator FadePhase()
    {
        inGameUI.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);

        StageManager.Instance.PlayerInput.enabled = false;
        yield return new WaitForSecondsRealtime(1f);
        fadeImage.DOFade(1f, 0.75f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1.5f);
    }

    private IEnumerator DoorPhase()
    {
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
    }

    private IEnumerator CameraPhase()
    {
        cam.GetComponent<FollowCamera>().enabled = false;
        cam.transform.position = startPoint.position;
        cam.transform.rotation = startPoint.rotation;
        cam.fieldOfView = 30f;

        doorLeft.DOAnchorPosX(-doorLeft.rect.width, duration);
        doorRight.DOAnchorPosX(doorRight.rect.width, duration);
        yield return new WaitForSecondsRealtime(duration);
        foreach (Animator displayCharacter in displayCharacters)
        {
            displayCharacter.SetTrigger("Clear");
        }

        yield return new WaitForSecondsRealtime(2f);

        cam.transform.DOMove(endPoint.position, 0.7f);
        cam.transform.DORotate(endPoint.rotation.eulerAngles, 0.7f);
        yield return new WaitForSecondsRealtime(1.5f);
    }

    private IEnumerator ResultUIPhase()
    {
        string stageName = GameManager.Instance.stageData.stageName;
        float currentExp = 4635;
        float maxExp = 6000;

        panel.anchoredPosition = new Vector2(-panel.rect.width, panel.anchoredPosition.y);
        originalY = exp.anchoredPosition.y;
        exp.anchoredPosition = new Vector2(exp.anchoredPosition.x, originalY + 50f);

        victoryRect.localScale = Vector3.one * 5f;
        Color c = victoryText.color;
        c.a = 0f;
        victoryText.color = c;

        stageTitleGroup.alpha = 0f;
        expGroup.alpha = 0f;
        expFill.fillAmount = 0f;

        stageTitleText.text = stageName;
        expText.text = $"{currentExp}/{maxExp}";

        panel.gameObject.SetActive(true);

        panel.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutCubic);
        yield return new WaitForSecondsRealtime(1f);

        victoryText.DOFade(1f, 0.2f);
        yield return victoryRect.DOScale(Vector3.one, 0.2f).WaitForCompletion();
        victoryRect.DOPunchScale(Vector3.one * -0.05f, 1.5f, 3, 0.3f);
        yield return new WaitForSecondsRealtime(0.5f);

        stageTitleGroup.DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(0.5f);

        expGroup.DOFade(1, 0.5f);
        exp.DOAnchorPosY(originalY, 0.5f);
        yield return new WaitForSecondsRealtime(1f);

        expFill.DOFillAmount(currentExp / maxExp, 1f);
        yield return new WaitForSecondsRealtime(1.5f);

        endPanel.gameObject.SetActive(true);
        endPanel.DOFade(1f, 1f);
    }
}
