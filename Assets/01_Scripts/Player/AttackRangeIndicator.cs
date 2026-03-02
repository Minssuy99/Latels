using UnityEngine;

public class AttackRangeIndicator : MonoBehaviour
{
    private void Start()
    {
        CharacterSetup setup = GetComponentInParent<CharacterSetup>();
        float scale = setup.AttackRange * 2f;
        transform.localScale = new Vector3(scale, 1, scale);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}