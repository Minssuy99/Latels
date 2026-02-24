using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private bool isBoss;
    public EnemyData Data => enemyData;
    public bool IsBoss => isBoss;
}