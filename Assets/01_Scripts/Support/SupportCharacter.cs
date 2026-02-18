using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SupportCharacter : MonoBehaviour
{
    protected List<EnemyStateManager> enemies;
    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected abstract IEnumerator SkillSequence();

    public void Initialize(List<EnemyStateManager> enemies)
    {
        this.enemies = enemies;
        StartCoroutine(SkillSequence());
    }
}
