using UnityEngine;

public class DamagePopupUI : MonoBehaviour
{
    private GameObject damagePrefab;

    public void SpawnDamagePopup(float damage, Transform target, Vector3 attackerPos, DamageType type)
    {
        if (!damagePrefab)
        {
            damagePrefab = Resources.Load<GameObject>("Damage");
        }
        GameObject obj = PoolManager.Instance.Get(damagePrefab);
        obj.transform.SetParent(transform, false);
        obj.GetComponent<Damage>().SetDamage(damage, target, attackerPos, type);
    }
}