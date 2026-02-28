using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListItem : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject selectedBadge;
    [SerializeField] private TMP_Text selectedText;

    public CharacterData Data { get; private set; }
    private Button button;

    public void Setup(CharacterData data, Action<CharacterData> onClick)
    {
        Data = data;
        profileImage.sprite = data.sprites.profileImage;
        nameText.text = data.charName;

        button = GetComponent<Button>();
        button.onClick.AddListener(() => onClick(Data));
    }

    public void SetSelected(bool selected, string text = "")
    {
        selectedBadge.SetActive(selected);
        if (selected)
        {
            selectedText.text = text;
        }
    }
}
