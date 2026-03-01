using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyHitEffect : MonoBehaviour
{
    private Renderer[] renderers;
    private Material[][] originalMaterials;
    [SerializeField] private Material HitMaterial;
    [SerializeField] private Transform rootModel;
    [SerializeField] private float shakeStrength = 0.2f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private int shakeVibrato = 1;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        HitMaterial = new Material(HitMaterial);
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void PlayHitEffect()
    {
        HitShake();
        HitFlash();
    }

    private void HitShake()
    {
        rootModel.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, shakeStrength), shakeVibrato).SetUpdate(UpdateType.Late);
    }

    private void HitFlash()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            int count = originalMaterials[i].Length;
            Material[] newMats = new Material[count];

            for (int j = 0; j < count; j++)
            {
                newMats[j] = HitMaterial;
            }
            renderers[i].materials = newMats;
        }

        StartCoroutine(RestoreMaterials());
    }

    private IEnumerator RestoreMaterials()
    {
        HitMaterial.SetColor("_BaseColor", Color.red);
        yield return new WaitForSecondsRealtime(0.1f);

        HitMaterial.SetColor("_BaseColor", Color.white);
        yield return new WaitForSecondsRealtime(0.1f);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }
    }
}
