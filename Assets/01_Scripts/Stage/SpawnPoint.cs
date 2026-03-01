using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private bool isBoss;
    [SerializeField] private bool visibleOnSpawn;
    public EnemyData Data => enemyData;
    public bool IsBoss => isBoss;
    public bool VisibleOnSpawn => visibleOnSpawn;
}