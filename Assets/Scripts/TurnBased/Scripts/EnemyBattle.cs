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

    [SerializeField] private GameObject _enemyCircle;

    [field: SerializeField] public List<BattleItemSO> ThisEnemyLoot { get; set; }

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
        DetermineObtainableItems();
    }

    private void DetermineObtainableItems()
    {
        System.Random rnd = new();
        foreach (BattleItemSO item in _enemyData.PossibleLoot)
        {
            var prob = rnd.NextDouble();
            if (prob < item.AppearanceProbability)
            {
                ThisEnemyLoot.Add(item);
            }
        }
    }

    public override void Attack(CharacterBattle target, float damage, Action onAttackComplete)
    {
        target.TakeDamage(_enemyData.EnemyBaseAttack);
        if (target.IsDead())
        {
            BattleHandler.GetInstance().PlayerLost();
            return;
        }
        onAttackComplete();
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
