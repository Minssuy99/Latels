using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CharacterSelectScreen : UIScreen
{
    [Header("※ Camera")]
    [SerializeField] private Transform camDefaultPos;
    [SerializeField] private Transform camPanelPos;

    [Space(10)]
    [Header("※ Character Slot")]
    [SerializeField] private RectTransform characterPanel;
    [SerializeField] private TMP_Text[] characterName;
    [SerializeField] private GameObject[] characterPosition;
    [SerializeField] private GameObject[] nameTags;
    [SerializeField] private GameObject[] emptyIcons;

    [Space(10)]
    [Header("※ Button")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject backButtonPanel;

    [Space(10)]
    [Header("※ List Panel")]
    [SerializeField] private RectTransform characterList;
    [SerializeField] private RectTransform pickIndicator;
    [SerializeField] private RectTransform[] touchPanels;
    [SerializeField] private GameObject listItemPrefab;
    [SerializeField] private Transform listContent;

    [Space(10)]
    [Header("※ Alert")]
    [SerializeField] private CanvasGroup alertCanvasGroup;

    [Space(10)]
    [Header("※ Reference")]
    [SerializeField] private UIScreen stageScreen;

    private Camera cam;
    private ScrollRect characterListScroll;
    private CharacterData[] ownedCharacters;
    private GameObject[] displayModels = new GameObject[3];
    private List<CharacterListItem> listItems = new();
    private Tweener pickRotation;
    private float defaultFOV;
    private bool isPanelOpen;
    private int selectedSlotIndex = -1;

    private void Awake()
    {
        cam = Camera.main;
        characterListScroll = GetComponentInChildren<ScrollRect>();
        ownedCharacters = Resources.LoadAll<CharacterData>("Characters");
        alertCanvasGroup.alpha = 0;
    }

    public override void OnEnter(Action onComplete)
    {
        gameObject.SetActive(true);
        isPanelOpen = false;
        pickIndicator.gameObject.SetActive(false);

        cam.transform.position = camDefaultPos.position;
        characterPanel.DOAnchorPosX(0, 0.25f);
        characterPanel.DOScale(new Vector3(1f, 1f), 0.25f);

        float aspect = (float)Screen.width / Screen.height;
        cam.fieldOfView = Mathf.Lerp(35, 27, Mathf.InverseLerp(1.33f, 2.17f, aspect));
        defaultFOV = cam.fieldOfView;

        float listWidth = Mathf.Lerp(730, 900, Mathf.InverseLerp(1.33f, 2.17f, aspect));
        characterList.sizeDelta = new Vector2(listWidth, characterList.sizeDelta.y);

        RefreshCharacterModels();

        startButton.SetActive(true);
        confirmButton.SetActive(false);
        characterList.gameObject.SetActive(false);
        backButtonPanel.gameObject.SetActive(false);
        characterList.DOAnchorPosX(830f, 0.25f);

        onComplete?.Invoke();
    }

    public override void OnExit(Action onComplete)
    {
        if (onComplete != null)
        {
            FadeManager.Instance.PlayFade(FadeDirection.LeftToRight, () =>
            {
                characterListScroll.verticalNormalizedPosition = 1;
                gameObject.SetActive(false);
                onComplete.Invoke();
            }, 1);
        }
        else
        {
            characterListScroll.verticalNormalizedPosition = 1;
            gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (GameManager.Instance.characterSlots[0] == null)
        {
            ShowAlert();
            return;
        }

        GameManager.Instance.returnToStage = true;
        GameManager.Instance.LoadGameScene(GameManager.Instance.stageData);
    }

    public void ShowCharacterSelectPanel(int index)
    {
        selectedSlotIndex = index;
        UpdatePickIndicator();

        if (isPanelOpen) return;
        isPanelOpen = true;
        PopulateList();

        cam.transform.DOMove(camPanelPos.position, 0.25f);
        cam.transform.DORotate(camPanelPos.rotation.eulerAngles, 0.25f);
        cam.DOFieldOfView(defaultFOV + 3f, 0.25f);

        characterPanel.DOAnchorPosX(-360, 0.25f);
        characterPanel.DOScale(new Vector3(0.85f, 0.85f), 0.25f);
        startButton.SetActive(false);
        confirmButton.SetActive(true);
        backButtonPanel.gameObject.SetActive(true);
        characterList.gameObject.SetActive(true);
        characterList.DOAnchorPosX(0, 0.25f);
    }

    public void HideCharacterSelectPanel()
    {
        if (pickRotation != null) pickRotation.Kill();
        pickIndicator.gameObject.SetActive(false);

        isPanelOpen = false;
        cam.transform.DOMove(camDefaultPos.position, 0.25f);
        cam.transform.DORotate(camDefaultPos.rotation.eulerAngles, 0.25f);
        cam.DOFieldOfView(defaultFOV, 0.25f);

        characterPanel.DOAnchorPosX(0, 0.25f);
        characterPanel.DOScale(new Vector3(1f, 1f), 0.25f);
        startButton.SetActive(true);
        confirmButton.SetActive(false);
        backButtonPanel.gameObject.SetActive(false);
        characterList.DOAnchorPosX(900f, 0.25f).OnComplete(() =>
        {
            characterList.gameObject.SetActive(false);
        });
    }

    private void PopulateList()
    {
        foreach (var item in listItems)
        {
            Destroy(item.gameObject);
        }
        listItems.Clear();

        CharacterData[] slots = GameManager.Instance.characterSlots;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;
            CreateListItem(slots[i], true, i);
        }

        foreach (CharacterData data in ownedCharacters)
        {
            if (Array.Exists(slots, s => s == data)) continue;
            CreateListItem(data, false);
        }
    }

    private void CreateListItem(CharacterData data, bool isSelected, int slotIndex = -1)
    {
        GameObject obj = Instantiate(listItemPrefab, listContent);
        CharacterListItem item = obj.GetComponent<CharacterListItem>();
        item.Setup(data, OnListItemClicked);
        item.SetSelected(isSelected, slotIndex == 0 ? "Main" : "Support");
        listItems.Add(item);
    }

    private void OnListItemClicked(CharacterData data)
    {
        List<int> changedSlots = new();
        CharacterData[] slots = GameManager.Instance.characterSlots;
        int existingSlot = Array.IndexOf(slots, data);

        if (existingSlot == selectedSlotIndex)
        {
            changedSlots.Add(selectedSlotIndex);
            slots[selectedSlotIndex] = null;
        }
        else if (existingSlot >= 0)
        {
            changedSlots.Add(existingSlot);
            changedSlots.Add(selectedSlotIndex);
            slots[existingSlot] = slots[selectedSlotIndex];
            slots[selectedSlotIndex] = data;
        }
        else
        {
            changedSlots.Add(selectedSlotIndex);
            slots[selectedSlotIndex] = data;
        }

        RefreshCharacterModels(changedSlots);
        RefreshSelectedBadges();
    }

    private void RefreshCharacterModels(List<int> changedSlots = null)
    {
        CharacterData[] slots = GameManager.Instance.characterSlots;

        for (int i = 0; i < slots.Length; i++)
        {
            if (changedSlots != null && !changedSlots.Contains(i)) continue;

            if (displayModels[i] != null)
            {
                Destroy(displayModels[i]);
            }

            if (slots[i] != null)
            {
                displayModels[i] = Instantiate(slots[i].displayPrefab, characterPosition[i].transform);
                if (changedSlots != null && changedSlots.Contains(i))
                {
                    displayModels[i].GetComponent<Animator>().SetTrigger("Select");
                }
                characterName[i].text = slots[i].charName;
                emptyIcons[i].SetActive(false);
                nameTags[i].SetActive(true);
            }
            else
            {
                displayModels[i] = null;
                characterName[i].text = "";
                emptyIcons[i].SetActive(true);
                nameTags[i].SetActive(false);
            }
        }
    }

    private void RefreshSelectedBadges()
    {
        CharacterData[] slots = GameManager.Instance.characterSlots;
        foreach (var item in listItems)
        {
            int slotIndex = Array.IndexOf(slots, item.Data);

            if (slotIndex >= 0)
            {
                item.SetSelected(true, slotIndex == 0 ? "Main" : "Support");
            }
            else
            {
                item.SetSelected(false);
            }
        }
    }

    private void UpdatePickIndicator()
    {
        pickIndicator.gameObject.SetActive(true);
        pickIndicator.position = new Vector3(touchPanels[selectedSlotIndex].position.x, pickIndicator.position.y, 0);

        if(pickRotation != null) pickRotation.Kill();
        pickIndicator.rotation = Quaternion.identity;

        pickRotation = pickIndicator.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void ShowAlert()
    {
        alertCanvasGroup.alpha = 0;
        alertCanvasGroup.gameObject.SetActive(true);

        alertCanvasGroup.DOFade(1, 0.3f)
            .OnComplete(() =>
            {
                alertCanvasGroup.DOFade(0, 0.3f)
                    .SetDelay(2f)
                    .OnComplete(() =>
                    {
                        alertCanvasGroup.gameObject.SetActive(false);
                    });
            });
    }
}