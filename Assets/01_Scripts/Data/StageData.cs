using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "SO/StageData")]
public class StageData : ScriptableObject
{
    public int stageNumber = 0;
    public Vector2 stageScreenPosition;

    [Header("Map Settings")]
    public GameObject mapPrefab;
    public AreaSpawnData[] areas;
}