using System;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Enemy enemy = transform.GetChild(i).GetComponent<Enemy>();
            enemies.Add(enemy);

            enemy.OnDeath += () => enemies.Remove(enemy);
        }

        Debug.Log($"Enemy : " +  enemies.Count);
    }

    public List<Enemy> GetEnemies() => enemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(Enemy enemy in enemies)
            {
                enemy.enabled = true;
            }
        }
    }
}