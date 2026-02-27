using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, Vector3 attackerPos = default(Vector3));
}