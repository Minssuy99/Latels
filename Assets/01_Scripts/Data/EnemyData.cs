using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "SO/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    public GameObject prefab;
    public EnemyStats stats;
}

[System.Serializable]
public class EnemyStats
{
    public float moveSpeed;
    public float health;
    public float damage;
    public float attackRange;
    public float attackCooldown;
    public int superArmorCount;
}