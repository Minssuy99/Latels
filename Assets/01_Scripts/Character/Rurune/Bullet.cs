using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem projectileEffect;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;

    private float elapsedTime;
    private Vector3 moveDirection;
    private float damage;

    public void Init(Transform target, float damage, Vector3 shooterForward)
    {
        elapsedTime = 0f;
        this.damage = damage;
        moveDirection = target.position + Vector3.up - transform.position;
        if (moveDirection.sqrMagnitude < 1f)
        {
            moveDirection = shooterForward;
        }
        moveDirection = moveDirection.normalized;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        projectileEffect.Play();
    }

    private void Update()
    {
        elapsedTime += TimeManager.Instance.PlayerDelta;

        if (elapsedTime > lifeTime)
        {
            projectileEffect.Stop();
            PoolManager.Instance.Return(gameObject);
            return;
        }

        float moveDistance = speed * TimeManager.Instance.PlayerDelta;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, moveDirection, moveDistance);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<IDamageable>().TakeDamage(damage, transform.position);

                GameObject hitVFX = PoolManager.Instance.Get(hitEffect);
                hitVFX.transform.position = hit.collider.transform.position + Vector3.up;
                hitVFX.transform.rotation = Quaternion.LookRotation(-moveDirection) * Quaternion.Euler(90, 0, 0);;
                PoolManager.Instance.Return(hitVFX, 2f);
            }
        }

        transform.position += moveDirection * moveDistance;
    }
}