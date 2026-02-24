using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterItem : MonoBehaviour
{
    [SerializeField] private Image chapterImage;
    [SerializeField] private TMP_Text titleText;

    public void Setup(ChapterData data)
    {
        chapterImage.sprite = data.chapterImage;
        titleText.text = data.chapterNumber.ToString();
    }
}
