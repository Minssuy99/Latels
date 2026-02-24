using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "SO/StageData")]
public class StageData : ScriptableObject
{
    public int stageNumber;
    public string stageName;
    public Vector2 stageScreenPosition;

    [Header("Map Settings")]
    public GameObject mapPrefab;
}