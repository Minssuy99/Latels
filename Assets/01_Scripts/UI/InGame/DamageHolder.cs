using UnityEngine;

public class DamageHolder : MonoBehaviour
{
    [SerializeField] private GameObject damagePrefab;

    public void SpawnDamagePopup(float damage, Transform target, Vector3 attackerPos, DamageType type)
    {
        GameObject obj = PoolManager.Instance.Get(damagePrefab);
        obj.transform.SetParent(transform, false);
        obj.GetComponent<Damage>().SetDamage(damage, target, attackerPos, type);
    }
}