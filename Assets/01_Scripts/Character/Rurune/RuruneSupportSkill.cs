using UnityEngine;
  using System.Collections;
  using System.Collections.Generic;

  public class RuruneSupportSkill : MonoBehaviour, ISupportSkill
  {
      private Animator animator;
      private TargetDetector targetDetector;
      private RuruneAttack ruruneAttack;

      public void Initialize()
      {
          if (animator == null)
          {
              animator = GetComponent<Animator>();
              targetDetector = GetComponent<TargetDetector>();
              ruruneAttack = GetComponent<RuruneAttack>();
          }
      }

      public void OnSkillStart()
      {
          StartCoroutine(Run());
      }

      private IEnumerator Run()
      {
          yield return StartCoroutine(SkillSequence());
          gameObject.SetActive(false);
      }

      private IEnumerator SkillSequence()
      {
          animator.updateMode = AnimatorUpdateMode.UnscaledTime;
          GameObject target1 = targetDetector.FindNearestTarget()?.gameObject;

          if (target1 != null)
              transform.LookAt(target1.transform);

          yield return new WaitForSecondsRealtime(0.1f);
          // animator.SetTrigger("Kick1");
          yield return new WaitForSecondsRealtime(0.8f);

          GameObject target2 = targetDetector.FindNearestTarget(target1)?.gameObject;
          if (target2 == null && target1 != null && target1.GetComponent<EnemyStateManager>().health.HP > 0)
          {
              target2 = target1;
          }

          if (target2 != null)
              transform.LookAt(target2.transform);

          yield return new WaitForSecondsRealtime(0.1f);
          // animator.SetTrigger("Kick2");
          yield return new WaitForSecondsRealtime(1.675f);
          animator.updateMode = AnimatorUpdateMode.Normal;
      }

      public void OnRuruneSkill()
      {
          if (!enabled) return;
          if (ruruneAttack == null)
              ruruneAttack = GetComponent<RuruneAttack>();
          ruruneAttack.Shoot();
      }
  }