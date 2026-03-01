using System;
using UnityEngine;
using System.Collections.Generic;

public class Area : MonoBehaviour
{
    public Action onCleared;

    private GameObject gate;
    private EnemyStateManager boss;
    private SpawnPoint[] spawnPoints;
    private readonly List<EnemyStateManager> enemies = new();
    private bool isEnter;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
        gate = transform.Find("Gate")?.gameObject;
        if (gate != null)
        {
            gate.SetActive(true);
        }
    }

    public void SpawnEnemies(Transform parent)
    {
        foreach (SpawnPoint point in spawnPoints)
        {
            GameObject enemyObj = Instantiate(point.Data.prefab, point.transform.position, point.transform.rotation);
            enemyObj.transform.SetParent(parent, true);
            EnemyStateManager enemy = enemyObj.GetComponent<EnemyStateManager>();

            enemy.SetData(point.Data);
            enemy.SetArea(this);
            enemy.gameObject.name = point.Data.EnemyName;
            enemies.Add(enemy);
            InGameUIManager.Instance.SubscribeEnemy(enemy.health);

            if (point.IsBoss)
            {
                enemy.gameObject.name = $"보스: {point.Data.EnemyName}";
                boss = enemy;
            }

            if (!point.VisibleOnSpawn)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public void OnPlayerEnter(Collider other)
    {
        if (isEnter) return;

        if (other.CompareTag(GameTags.Player))
        {
            isEnter = true;

            if (boss != null)
            {
                InGameUIManager.Instance.ShowBossHp(boss.health);
            }

            foreach(EnemyStateManager enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
                enemy.Activate();
            }
        }
    }

    public void RemoveEnemy(EnemyStateManager enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            gate.SetActive(false);
            onCleared?.Invoke();

            if (boss)
            {
                InGameUIManager.Instance.HideBossHp();
            }
        }
    }
}