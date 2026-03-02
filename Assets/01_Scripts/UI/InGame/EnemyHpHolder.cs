using UnityEngine;

public class EnemyHpHolder : MonoBehaviour
{
    [SerializeField] private GameObject enemyHpBarPrefab;

    public void CreateHpBar(EnemyHealth enemy)
    {
        GameObject obj = PoolManager.Instance.Get(enemyHpBarPrefab);
        obj.transform.SetParent(transform);
        obj.GetComponent<EnemyHpBar>().SetTarget(enemy);
    }
}