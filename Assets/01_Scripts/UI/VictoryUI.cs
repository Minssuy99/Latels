using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;

public class VictoryUI : MonoBehaviour
{
    [Header("패널")]
    [SerializeField] private RectTransform panel;

    [Header("요소")]
    [SerializeField] private RectTransform victoryText;
    [SerializeField] private CanvasGroup stageTitleGroup;
    [SerializeField] private TMP_Text stageTitleText;
    [SerializeField] private RectTransform exp;
    [SerializeField] private Image expFill;
    [SerializeField] private TMP_Text expText;

    [Header("로비 복귀")]
    [SerializeField] private CanvasGroup EndPanel;

    private CanvasGroup expGroup;
    private float originalY;

    private void Awake()
    {
        expGroup = exp.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        EndPanel.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);

        EndPanel.alpha = 0f;

        Button continueButton = EndPanel.GetComponent<Button>();

        continueButton.onClick.AddListener(() =>
        {
            InGameUIManager.Instance.enabled = false;
            FadeManager.Instance.PlayFade(FadeDirection.LeftToRight, () =>
            {
                GameManager.Instance.LoadLobbyScene();
            }, 2f);
        });
    }

    public void Show(string stageName, float currentExp, float maxExp)
    {
        panel.anchoredPosition = new Vector2(-panel.rect.width, panel.anchoredPosition.y);
        originalY = exp.anchoredPosition.y;
        exp.anchoredPosition = new Vector2(exp.anchoredPosition.x, originalY + 50f);

        victoryText.localScale = Vector3.one * 5f;
        Color c = victoryText.GetComponent<TMP_Text>().color;
        c.a = 0f;
        victoryText.GetComponent<TMP_Text>().color = c;

        stageTitleGroup.alpha = 0f;
        expGroup.alpha = 0f;
        expFill.fillAmount = 0f;

        stageTitleText.text = stageName;
        expText.text = $"{currentExp}/{maxExp}";

        panel.gameObject.SetActive(true);
        StartCoroutine(PlaySequence(currentExp, maxExp));
    }

    IEnumerator PlaySequence(float currentExp, float maxExp)
    {
        panel.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutCubic);
        yield return new WaitForSecondsRealtime(1f);

        victoryText.GetComponent<TMP_Text>().DOFade(1f, 0.2f);
        yield return victoryText.DOScale(Vector3.one, 0.2f).WaitForCompletion();
        victoryText.DOPunchScale(Vector3.one * -0.05f, 1.5f, 3, 0.3f);
        yield return new WaitForSecondsRealtime(0.5f);

        stageTitleGroup.DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(0.5f);

        expGroup.DOFade(1, 0.5f);
        exp.DOAnchorPosY(originalY, 0.5f);
        yield return new WaitForSecondsRealtime(1f);

        expFill.DOFillAmount(currentExp / maxExp, 1f);
        yield return new WaitForSecondsRealtime(1.5f);

        EndPanel.gameObject.SetActive(true);
        EndPanel.DOFade(1f, 1f);
    }
}