using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public override void Attack(CharacterBattle target, Action onAttackComplete)
    {
        target.TakeDamage(_mainCharData.BaseAttack);
        if (target.IsDead())
        {
            BattleHandler.GetInstance().RemoveEnemy(target as EnemyBattle);
        }
        onAttackComplete();
    }
}