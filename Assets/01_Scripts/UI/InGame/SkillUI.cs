using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SkillSlotUI
{
    public Image background;
    public Image icon;
    public Image line;
    public TMP_Text cooldownText;
}

public class SkillUI : MonoBehaviour
{
    [SerializeField] private SkillSlotUI mainSkillSlot;
    [SerializeField] private SkillSlotUI support1SkillSlot;
    [SerializeField] private SkillSlotUI support2SkillSlot;

    private PlayerStateManager player;

    private void Start()
    {
        mainSkillSlot.cooldownText.gameObject.SetActive(false);
        support1SkillSlot.cooldownText.gameObject.SetActive(false);
        support2SkillSlot.cooldownText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!player) return;
        UpdateCooldownUI(mainSkillSlot, player.mainSkill.remainTime);
        UpdateCooldownUI(support1SkillSlot, player.supportSkill.GetRemainTime(0));
        UpdateCooldownUI(support2SkillSlot, player.supportSkill.GetRemainTime(1));
    }

    private void UpdateCooldownUI(SkillSlotUI slot, float remainTime)
    {
        if (remainTime > 0)
        {
            SetImageAlpha(slot.background, 0.4f);
            SetImageAlpha(slot.icon, 0.4f);
            SetImageAlpha(slot.line, 0.4f);

            slot.cooldownText.gameObject.SetActive(true);
            slot.cooldownText.text = Mathf.CeilToInt(remainTime).ToString();

            if (remainTime < 1)
            {
                slot.cooldownText.text = remainTime.ToString("F1");
            }
        }
        else
        {
            SetImageAlpha(slot.background, 0f);
            SetImageAlpha(slot.icon, 1f);
            SetImageAlpha(slot.line, 1f);

            slot.cooldownText.gameObject.SetActive(false);
        }
    }

    public void SetPlayer(PlayerStateManager player)
    {
        this.player = player;
        if (GameManager.Instance == null) return;

        CharacterData mainData = GameManager.Instance.characterSlots[0];
        if (mainData != null)
            mainSkillSlot.icon.sprite = mainData.sprites.skillIcon;

        CharacterData sub1Data = GameManager.Instance.characterSlots[1];
        if (sub1Data != null)
            support1SkillSlot.icon.sprite = sub1Data.sprites.skillIcon;

        CharacterData sub2Data = GameManager.Instance.characterSlots[2];
        if (sub2Data != null)
            support2SkillSlot.icon.sprite = sub2Data.sprites.skillIcon;
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color temp = image.color;
        temp.a = alpha;
        image.color = temp;
    }
}