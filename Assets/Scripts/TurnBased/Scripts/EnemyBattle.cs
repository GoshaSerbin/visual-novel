using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : CharacterBattle
{
    [SerializeField] private EnemyData _enemyData = null;

    public override bool AttackBuffActive { get; set; }
    public override bool DefenseBuffActive { get; set; }

    public override HealthSystem HealthComponent { get; set; }

    public Sprite GetSprite()
    {
        return _enemyData.EnemySprite;
    }
    public string GetEnemyName()
    {
        return _enemyData.EnemyName;
    }

    public void Setup(EnemyData enemyData)
    {
        _enemyData = enemyData;
        HealthComponent = new HealthSystem(enemyData.EnemyHealthMax);
        AttackBuffActive = false;
        DefenseBuffActive = false;
    }

    public override void Attack(CharacterBattle target, Action onAttackComplete)
    {
        target.TakeDamage(_enemyData.EnemyBaseAttack);
        onAttackComplete();
    }
}
