using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SupportSlot
{
    public GameObject characterObj;
    public ISupportSkill support;
    public float coolTime;
    public float remainTime;
    public bool canUse;
}

public class SupportSkill : MonoBehaviour, ISkillComponent
{
    private PlayerStateManager player;
    private SupportSlot[] slots = new SupportSlot[2];

    private void Awake()
    {
        player = GetComponent<PlayerStateManager>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new SupportSlot();
        }
    }

    private void Start()
    {
        StartCoroutine(Warmup());
    }

    private void Update()
    {
        foreach (SupportSlot slot in slots)
        {
            if (!slot.canUse)
            {
                slot.remainTime -= TimeManager.Instance.PlayerDelta;
                if (slot.remainTime <= 0)
                {
                    slot.canUse = true;
                }
            }
        }
    }

    private void ActivateSupport(int index)
    {
        SupportSlot slot = slots[index];

        if (player.IsDead) return;
        if (!slot.canUse) return;
        if (player.attack.GetEnemies().Count == 0) return;

        slot.canUse = false;
        slot.remainTime = slot.coolTime;

        if(slot.characterObj == null) return;
        slot.characterObj.transform.position = player.transform.position;
        slot.characterObj.SetActive(true);
        slot.support.Initialize(player.attack.GetEnemies());
        slot.support.OnSkillStart();
    }

    public void OnSubSkill_1(InputValue value)
    {
        ActivateSupport(0);
    }
    public void OnSubSkill_2(InputValue value)
    {
        ActivateSupport(1);
    }

    IEnumerator Warmup()
    {
        yield return null;
        foreach (SupportSlot slot in slots)
        {
            if (slot.characterObj == null) continue;
            slot.characterObj.SetActive(true);
        }

        yield return null;
        foreach (SupportSlot slot in slots)
        {
            if (slot.characterObj == null) continue;
            slot.characterObj.SetActive(false);
        }
    }

    public void SetSupport(int index, GameObject support)
    {
        slots[index].characterObj = support;
        slots[index].support = support.GetComponent<ISupportSkill>();
        slots[index].coolTime = support.GetComponent<CharacterSetup>().Data.stats.skillCoolTime;
    }

    public float GetRemainTime(int index)
    {
        return slots[index].remainTime;
    }
}
