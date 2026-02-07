using System;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();

    [SerializeField] private GameObject bossUI;
    [SerializeField] private GameObject Entrance;
    [SerializeField] private GameObject Exit;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyStateManager enemy = transform.GetChild(i).GetComponent<EnemyStateManager>();
            if (enemy != null)
                enemies.Add(enemy);
        }

        Debug.Log($"Enemy : " +  enemies.Count);

        Entrance.SetActive(false);
        Exit.SetActive(true);
    }

    public List<EnemyStateManager> GetEnemies() => enemies;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter: {other.name}");
        if (other.CompareTag("Player"))
        {
            if(bossUI != null)
                bossUI.SetActive(true);

            Entrance.SetActive(true);
            foreach(EnemyStateManager enemy in enemies)
            {
                enemy.Activate(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemies.Count == 0)
            {
                Exit.SetActive(true);
            }
        }
    }

    public void RemoveEnemy(EnemyStateManager enemy)
    {
        enemies.Remove(enemy);
        Debug.Log("남은 적 수 : " + enemies.Count);

        if (enemies.Count == 0)
        {
            Exit.SetActive(false);
        }
    }
}