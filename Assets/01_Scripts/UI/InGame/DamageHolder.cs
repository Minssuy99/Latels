using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageHolder : MonoBehaviour
{
    [SerializeField] private GameObject damagePrefab;

    private readonly Dictionary<EnemyHealth, Action<float, Vector3>> handlers = new();

    public void SpawnDamagePopup(float damage, Transform target, Vector3 attackerPos, DamageType type)
    {
        GameObject obj = PoolManager.Instance.Get(damagePrefab);
        obj.transform.SetParent(transform, false);
        obj.GetComponent<Damage>().SetDamage(damage, target, attackerPos, type);
    }

    public void SubscribeEnemy(EnemyHealth enemy)
    {
        Action<float, Vector3> handler = (damage, attackPos) =>
        {
            SpawnDamagePopup(damage, enemy.transform, attackPos, DamageType.Enemy);
        };
        handlers[enemy] = handler;
        enemy.OnDamaged += handler;
    }

    public void UnsubscribeEnemy(EnemyHealth enemy)
    {
        if (handlers.ContainsKey(enemy))
        {
            enemy.OnDamaged -= handlers[enemy];
            handlers.Remove(enemy);
        }
    }
}