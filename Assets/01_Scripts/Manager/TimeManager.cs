using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeManager : Singleton<TimeManager>
{
    [Header("Time Settings")]
    [SerializeField] private float slowScale = 0.1f;
    [SerializeField] private float playerHitStopScale = 0.3f;
    [SerializeField] private float hitStopDuration = 0.5f;
    [SerializeField] private float bulletTimeCooldown = 15f;
    private float bulletTimeCooldownTimer = 0f;
    private bool CanBulletTime => bulletTimeCooldownTimer <= 0f;

    [Header("시각효과")]
    [SerializeField] private Volume globalVolume;
    private Bloom bloom;
    private float baseFixedDeltaTime;
    private bool isHitStop = false;

    /* Property */
    public float PlayerDelta => isHitStop ? Time.unscaledDeltaTime * playerHitStopScale : Time.unscaledDeltaTime;
    public float EnemyDelta => Time.deltaTime;

    private void Start()
    {
        baseFixedDeltaTime = Time.fixedDeltaTime;
        globalVolume.profile.TryGet(out bloom);
    }

    private void Update()
    {
        if (bulletTimeCooldownTimer > 0f)
        {
            bulletTimeCooldownTimer -= PlayerDelta;
        }
    }

    public void StartHitStop()
    {
        isHitStop = true;
        Time.timeScale = slowScale;
        Time.fixedDeltaTime = baseFixedDeltaTime * slowScale;
    }

    public void StopHitStop()
    {
        isHitStop = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = baseFixedDeltaTime;
    }

    public void BulletTime(Animator animator)
    {
        StartCoroutine(BulletTimeSequence(animator));
    }

    private IEnumerator BulletTimeSequence(Animator animator)
    {
        StartHitStop();
        yield return new WaitForSecondsRealtime(hitStopDuration);
        StopHitStop();

        if (CanBulletTime)
        {
            bulletTimeCooldownTimer = bulletTimeCooldown;
            bloom.tint.value = Color.red;
            Time.timeScale = slowScale;
            Time.fixedDeltaTime = baseFixedDeltaTime * slowScale;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            yield return new WaitForSecondsRealtime(4f);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = baseFixedDeltaTime;
            animator.updateMode = AnimatorUpdateMode.Normal;
            bloom.tint.value = Color.white;
        }
    }
}
