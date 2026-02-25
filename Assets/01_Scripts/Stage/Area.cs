using System;
using UnityEngine;
using System.Collections.Generic;

public class Area : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private EnemyStateManager boss;
    public SpawnPoint[] spawnPoints;

    public Action OnCleared;

    private List<EnemyStateManager> enemies = new();
    private bool isEnter;

    private void Start()
    {
        door.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnter) return;

        if (other.CompareTag("Player"))
        {
            isEnter = true;

            PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
            playerAttack.SetEnemies(enemies);

            if(boss != null)
                InGameUIManager.Instance.ShowBossHP(boss.attack);

            foreach(EnemyStateManager enemy in enemies)
            {
                enemy.Activate(this);
            }
        }
    }

    public void AddEnemy(EnemyStateManager enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyStateManager enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            door.SetActive(false);
            OnCleared?.Invoke();

            if (boss != null)
                InGameUIManager.Instance.HideBossHP();
        }
    }

    public void SetBoss(EnemyStateManager boss)
    {
        this.boss = boss;
    }
}