using System;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyStateManager enemy = transform.GetChild(i).GetComponent<EnemyStateManager>();
            enemies.Add(enemy);
        }

        Debug.Log($"Enemy : " +  enemies.Count);
    }

    public List<EnemyStateManager> GetEnemies() => enemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(EnemyStateManager enemy in enemies)
            {
                enemy.Activate();
            }
        }
    }
}