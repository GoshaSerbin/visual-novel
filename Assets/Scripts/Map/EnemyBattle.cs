using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : CharacterBattle
{
    [SerializeField] private EnemyData _enemyData = null;

    public override bool AttackBuffActive { get; set; }
    public override bool DefenseBuffActive { get; set; }

    public static Color circleInactive = new(255, 255, 255, 36);
    public static Color circleActive = new(255, 255, 255, 180);

    public override HealthSystem HealthComponent { get; set; }

    [SerializeField] private GameObject _enemyCircle;

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
        if (target.IsDead())
        {
            BattleHandler.GetInstance().PlayerLost();
        }
        else
        {
            onAttackComplete();
        }
    }

    public void OnMouseEnter()
    {
        ActivateCircle();
    }

    public void OnMouseExit()
    {
        if (BattleHandler.GetInstance().GetPlayerTarget() == this)
        {
            return;
        }
        DeactivateCircle();
    }

    public void OnMouseDown()
    {
        BattleHandler.GetInstance().PlayerTargetSelected(this);
    }

    public void ActivateCircle()
    {
        _enemyCircle.SetActive(true);
    }

    public void DeactivateCircle()
    {
        _enemyCircle.SetActive(false);
    }
}
