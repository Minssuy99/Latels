using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{
    public float skillCoolTime => playerData.stats.skillCoolTime;
    public float remainTime;

    private bool canUseSkill = true;

    private CharacterData playerData;
    protected PlayerStateManager playerState;

    private void Awake()
    {
        playerState = GetComponent<PlayerStateManager>();
    }

    private void Start()
    {
        playerData = GameManager.Instance.characterSlots[0];
    }

    private void Update()
    {
        if (canUseSkill == false)
        {
            remainTime -= TimeManager.Instance.PlayerDelta;

            if (remainTime <= 0)
                canUseSkill = true;
        }
    }

    public void OnMainSkill(InputValue value)
    {
        if (playerState.IsUsingSkill) return;
        if (playerState.IsDead) return;
        if (playerState.targetEnemy == null) return;
        if (canUseSkill == false) return;

        canUseSkill = false;
        remainTime = skillCoolTime;
        playerState.ChangeState(playerState.skillState);
    }
}