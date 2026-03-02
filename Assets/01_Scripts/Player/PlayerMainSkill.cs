  using UnityEngine;
  using UnityEngine.InputSystem;

  public abstract class PlayerMainSkill : MonoBehaviour, ISkillComponent
  {
      public float SkillCoolTime => player.CharacterData.stats.skillCoolTime;
      public float RemainTime { get; private set; }
      private bool canUseSkill = true;

      protected PlayerStateManager player;
      protected TargetDetector targetDetector;
      protected EnemyStateManager skillTarget;

      protected virtual void Awake()
      {
          player = GetComponent<PlayerStateManager>();
          targetDetector = GetComponent<TargetDetector>();
      }

      protected void Update()
      {
          if (canUseSkill == false)
          {
              RemainTime -= TimeManager.Instance.PlayerDelta;

              if (RemainTime <= 0)
                  canUseSkill = true;
          }
      }

      public void OnMainSkill(InputValue value)
      {
          if (player.IsUsingSkill) return;
          if (player.IsDead) return;
          if (player.targetEnemy == null) return;
          if (canUseSkill == false) return;

          canUseSkill = false;
          RemainTime = SkillCoolTime;
          player.ChangeState(player.skillState);
      }

      public virtual void OnSkillStart()
      {
          skillTarget = targetDetector.FindNearestTarget();

          if (skillTarget)
          {
              Vector3 direction = skillTarget.transform.position - transform.position;
              direction.y = 0;
              transform.rotation = Quaternion.LookRotation(direction);
          }

          player.animator.SetTrigger(AnimHash.Skill);
      }

      protected void EndSkill()
      {
          if (player.move.MoveDirection.sqrMagnitude > 0)
          {
              player.ChangeState(player.moveState);
          }
          else
          {
              player.ChangeState(player.idleState);
          }
      }
  }