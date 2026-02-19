using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SupportSkillManager : MonoBehaviour
{
    /* 지원캐릭터 1 설정 */
    private GameObject SubCharacter1;
    public float SubSkill_1_CoolTime = 15f;
    public float remain_SubSkill_1_CoolTime;
    private bool canSubSkill_1 = true;

    private PlayerStateManager player;
    private SupportCharacter support1;

    private void Awake()
    {
        player = GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        StartCoroutine(Warmup());
    }

    private void Update()
    {
        if (canSubSkill_1 == false)
        {
            remain_SubSkill_1_CoolTime -= TimeManager.Instance.PlayerDelta;

            if (remain_SubSkill_1_CoolTime <= 0)
                canSubSkill_1 = true;
        }
    }

    public void OnSubSkill_1(InputValue value)
    {
        if (player.IsDead) return;
        if (!canSubSkill_1) return;
        if (player.attack.GetEnemies().Count == 0) return;

        canSubSkill_1 = false;
        remain_SubSkill_1_CoolTime = SubSkill_1_CoolTime;

        SubCharacter1.transform.position = player.transform.position;
        SubCharacter1.SetActive(true);
        support1.Initialize(player.attack.GetEnemies());
    }

    IEnumerator Warmup()
    {
        yield return null;
        if (SubCharacter1 == null) yield break;
        SubCharacter1.SetActive(true);
        yield return null;
        SubCharacter1.SetActive(false);
    }

    public void SetSupport(GameObject support)
    {
        SubCharacter1 = support;
        support1 = support.GetComponent<SupportCharacter>();
    }
}
