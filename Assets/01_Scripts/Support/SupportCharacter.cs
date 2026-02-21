using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SupportCharacter : MonoBehaviour
{
    protected List<EnemyStateManager> enemies;
    protected Animator animator;

    private CharacterSetup setup;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        setup = GetComponent<CharacterSetup>();
    }

    protected abstract IEnumerator SkillSequence();

    public void Initialize(List<EnemyStateManager> enemies)
    {
        this.enemies = enemies;
        StartCoroutine(RunSkillSequence());
    }

    private IEnumerator RunSkillSequence()
    {
        yield return StartCoroutine(SkillSequence());
        if (setup.Role == CharacterRole.Support)
        {
            gameObject.SetActive(false);
        }
    }
}