using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    public bool IsSlowAll { get; private set; }
    public bool IsBulletTime { get; private set; }
    private Animator playerAnimator;
    private List<ParticleSystem> activeParticles = new();
    private Coroutine bulletTimeCoroutine;
    private bool isPaused;
    private float savedTimeScale;

    public float PlayerDelta
    {
        get
        {
            if (IsSlowAll)
                return Time.deltaTime;
            if (isHitStop)
                return Time.unscaledDeltaTime * playerHitStopScale;
            return Time.unscaledDeltaTime;
        }
    }
    public float EnemyDelta => Time.deltaTime;
    public bool IsSlowMotion => Time.timeScale < 1;
    public bool IsNormalMotion => Time.timeScale >= 1;
    public bool ParticleScaleMode => IsBulletTime;

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

    public void Pause()
    {
        isPaused = true;
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = savedTimeScale;
    }

    public IEnumerator WaitRealTime(float waitTime)
    {
        float elapsed = 0f;
        while (elapsed < waitTime)
        {
            if (!isPaused)
            {
                elapsed += Time.unscaledDeltaTime;
            }

            yield return null;
        }
    }

    public void StartHitStop()
    {
        isHitStop = true;

        if (playerAnimator)
            playerAnimator.speed = 1f;

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

    public void StartSlowAll(float holdDuration, float fadeInDuration = 0f, float fadeOutDuration = 0f)
    {
        StartCoroutine(SlowAllSequence(holdDuration, fadeInDuration, fadeOutDuration));
    }

    private IEnumerator SlowAllSequence(float holdDuration, float fadeInDuration, float fadeOutDuration)
    {
        IsSlowAll = true;
        RefreshAnimatorMode();
        if (playerAnimator) playerAnimator.speed = 1f;

        if (fadeInDuration > 0f)
            yield return StartCoroutine(LerpTimeScale(slowScale, fadeInDuration));
        else
        {
            Time.timeScale = slowScale;
            Time.fixedDeltaTime = baseFixedDeltaTime * slowScale;
        }

        yield return WaitRealTime(holdDuration);

        IsSlowAll = false;
        if (fadeOutDuration > 0f)
            yield return StartCoroutine(LerpTimeScale(1f, fadeOutDuration));
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = baseFixedDeltaTime;
        }

        RefreshAnimatorMode();
    }

    private IEnumerator LerpTimeScale(float time, float duration)
    {
        float start = Time.timeScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (!isPaused)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                Time.timeScale = Mathf.Lerp(start, time, t);
                Time.fixedDeltaTime = baseFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }

    private void RefreshAnimatorMode()
    {
        if (!playerAnimator) return;

        if (IsSlowAll)
        {
            playerAnimator.updateMode = AnimatorUpdateMode.Normal;
        }
        else if (IsBulletTime)
        {
            playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        else
        {
            playerAnimator.updateMode = AnimatorUpdateMode.Normal;
        }
    }

    public void PlayBulletTime(Animator animator)
    {
        bulletTimeCoroutine = StartCoroutine(BulletTimeSequence(animator));
    }

    public void StopBulletTime()
    {
        if (!IsBulletTime) return;

        if (bulletTimeCoroutine != null)
            StopCoroutine(bulletTimeCoroutine);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = baseFixedDeltaTime;
        IsBulletTime = false;
        isHitStop = false;
        RefreshAnimatorMode();
        RefreshParticleMode();
        bloom.tint.value = Color.white;
    }

    private IEnumerator BulletTimeSequence(Animator animator)
    {
        StartHitStop();
        yield return WaitRealTime(hitStopDuration);
        StopHitStop();

        if (CanBulletTime)
        {
            bulletTimeCooldownTimer = bulletTimeCooldown;
            IsBulletTime = true;
            bloom.tint.value = Color.red;
            Time.timeScale = slowScale;
            Time.fixedDeltaTime = baseFixedDeltaTime * slowScale;
            RefreshParticleMode();
            RefreshAnimatorMode();
            yield return WaitRealTime(4f);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = baseFixedDeltaTime;
            IsBulletTime = false;
            RefreshAnimatorMode();
            RefreshParticleMode();
            bloom.tint.value = Color.white;
        }
    }

    public void PlayParticle(GameObject obj)
    {
        var particles = obj.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
        {
            var main = ps.main;
            main.useUnscaledTime = ParticleScaleMode;
            activeParticles.Add(ps);
        }
        if (particles.Length > 0)
            particles[0].Play();
    }

    private void RefreshParticleMode()
    {
        for (int i = activeParticles.Count - 1; i >= 0; i--)
        {
            if (!activeParticles[i] || !activeParticles[i].gameObject.activeInHierarchy)
            {
                activeParticles.RemoveAt(i);
                continue;
            }
            var main = activeParticles[i].main;
            main.useUnscaledTime = ParticleScaleMode;
        }
    }
}
