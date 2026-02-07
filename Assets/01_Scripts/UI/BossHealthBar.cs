using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyAttack boss;
    [SerializeField] private Image currentBar;
    [SerializeField] private Image nextBar;

    [SerializeField] private Color[] barColors;
    [SerializeField] private float hpPerBar = 200f;

    private float maxHP;

    private void Start()
    {
        maxHP = boss.HP;
    }

    private void Update()
    {
        if (boss.HP <= 0)
        {
            currentBar.fillAmount = 0f;
            return;
        }

        int currentBarIndex = Mathf.CeilToInt(boss.HP / hpPerBar) - 1;

        float currentBarHP = boss.HP % hpPerBar;

        if (currentBarHP == 0)
        {
            currentBarHP = hpPerBar;
        }

        float fillAmount = currentBarHP / hpPerBar;

        currentBar.fillAmount = fillAmount;
        currentBar.color = barColors[currentBarIndex];

        if (currentBarIndex > 0)
        {
            nextBar.color = barColors[currentBarIndex - 1];
        }
        else
        {
            nextBar.color = Color.clear;
        }
    }
}