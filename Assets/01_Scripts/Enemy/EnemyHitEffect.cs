using UnityEngine;
using System.Collections;

public class EnemyHitEffect : MonoBehaviour
{
    private Renderer[] renderers;
    private Material[][] originalMaterials;
    [SerializeField] private Material HitMaterial;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void HitFlash()
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

    IEnumerator RestoreMaterials()
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
