using UnityEngine;
using UnityEngine.UI;

public enum ChapterNumber
{
    Chapter1,
    Chapter2,
    Chapter3,
    Chapter4,
    Chapter5,
    Chapter6,
    Chapter7,
    Chapter8,
}

[CreateAssetMenu(fileName = "Chapter", menuName = "SO/ChapterData")]
public class ChapterData : ScriptableObject
{
    public ChapterNumber chapterNumber;
    public Sprite chapterImage;
    public StageData[] stages;
}