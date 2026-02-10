using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BulletTimeManager : Singleton<BulletTimeManager>
{
    [Header("불렛타임")]
    [SerializeField] private float slowScale = 0.1f;
    [HideInInspector] public bool playerSlowDown = false;

    [Header("시각효과")]
    [SerializeField] private Volume globalVolume;
    private Bloom bloom;
    private float baseFixedDeltaTime;

    private void Start()
    {
        baseFixedDeltaTime = Time.fixedDeltaTime;
        globalVolume.profile.TryGet(out bloom);
    }

    public IEnumerator StartBulletTime(Animator animator)
    {
        playerSlowDown = false;
        bloom.tint.value = Color.red;
        Time.timeScale = slowScale;
        Time.fixedDeltaTime = baseFixedDeltaTime * slowScale;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitForSecondsRealtime(4f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = baseFixedDeltaTime;
        animator.updateMode = AnimatorUpdateMode.Normal;
        bloom.tint.value = Color.white;
        playerSlowDown = false;
    }

    public IEnumerator StartHitStop()
    {
        yield return null;
    }
}
