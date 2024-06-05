using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;

public class MainCharBattle : CharacterBattle
{
    [SerializeField] private MainCharData _mainCharData;

    public override bool AttackBuffActive { get; set; }
    public override bool DefenseBuffActive { get; set; }

    public override HealthSystem HealthComponent { get; set; }

    public void Setup()
    {
        HealthComponent = new HealthSystem(_mainCharData.MaxHP);
        AttackBuffActive = false;
        DefenseBuffActive = false;
    }
    public override void Attack(CharacterBattle target, float damage, Action onAttackComplete)
    {
        target.TakeDamage(_mainCharData.BaseAttack * Mathf.CeilToInt(damage));
        onAttackComplete();
        if (target.IsDead())
            EnemyDieWithDelay(target, 0.5f);
    }

    void EnemyDieWithDelay(CharacterBattle target, float delayTime)
    {
        StartCoroutine(DelayAction(target, delayTime));
    }

    IEnumerator DelayAction(CharacterBattle target, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        BattleHandler.GetInstance().RemoveEnemy(target as EnemyBattle);
    }
}
