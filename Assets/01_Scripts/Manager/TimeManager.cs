using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeManager : Singleton<TimeManager>
{
    [Header("※ Time Settings")]
    [SerializeField] private float slowScale = 0.05f;
    [SerializeField] private float playerHitStopScale = 0.6f;
    [SerializeField] private float hitStopDuration = 0.5f;
    [SerializeField] private float bulletTimeCooldown = 15f;
    private float bulletTimeCooldownTimer;
    private bool CanBulletTime => bulletTimeCooldownTimer <= 0f;

    [Header("※ Visual Effects")]
    [SerializeField] private Volume globalVolume;
    private Bloom bloom;
    private float baseFixedDeltaTime;
    private bool isHitStop;
    public bool IsBulletTime { get; private set; }
    private Animator playerAnimator;

    public float PlayerDelta => isHitStop ? Time.unscaledDeltaTime * playerHitStopScale : Time.unscaledDeltaTime;
    public float EnemyDelta => Time.deltaTime;

    private void Start()
    {
        baseFixedDeltaTime = Time.fixedDeltaTime;
        FindVolume();
    }

    private void FindVolume()
    {
        globalVolume = FindAnyObjectByType<Volume>();
        if (globalVolume != null)
            globalVolume.profile.TryGet(out bloom);
    }

    private void Update()
    {
        if (bulletTimeCooldownTimer > 0f)
        {
            bulletTimeCooldownTimer -= PlayerDelta;
        }
    }

    public void SetAnimator(Animator animator)
    {
        playerAnimator = animator;
        FindVolume();
    }

    public void StartHitStop()
    {
        isHitStop = true;
        Time.timeScale = slowScale;
        Time.fixedDeltaTime = baseFixedDeltaTime * slowScale;

        if (playerAnimator)
        {
            playerAnimator.speed = playerHitStopScale / slowScale;
        }
    }

    public void StopHitStop()
    {
        isHitStop = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = baseFixedDeltaTime;

        if (playerAnimator)
        {
            playerAnimator.speed = 1f;
        }
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
            IsBulletTime = true;
            bloom.tint.value = Color.red;
            Time.timeScale = slowScale;
            Time.fixedDeltaTime = baseFixedDeltaTime * slowScale;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            yield return new WaitForSecondsRealtime(4f);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = baseFixedDeltaTime;
            animator.updateMode = AnimatorUpdateMode.Normal;
            IsBulletTime = false;
            bloom.tint.value = Color.white;
        }
    }
}
