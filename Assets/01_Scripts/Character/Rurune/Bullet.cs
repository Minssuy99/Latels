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
    private RaycastHit[] hitBuffer = new RaycastHit[5];

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

        int count = Physics.RaycastNonAlloc(transform.position, moveDirection, hitBuffer, moveDistance);
        for (int i = 0; i < count; i++)
        {
            if (hitBuffer[i].collider.CompareTag(GameTags.Enemy))
            {
                hitBuffer[i].collider.GetComponent<IDamageable>().TakeDamage(damage, transform.position);

                GameObject hitVFX = PoolManager.Instance.Get(hitEffect);
                hitVFX.transform.position = hitBuffer[i].collider.transform.position + Vector3.up;
                hitVFX.transform.rotation = Quaternion.LookRotation(-moveDirection) * Quaternion.Euler(90, 0, 0);
                PoolManager.Instance.Return(hitVFX, 2f);
            }
        }

        transform.position += moveDirection * moveDistance;
    }
}