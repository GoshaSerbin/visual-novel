using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBattle : MonoBehaviour
{
    [SerializeField] public abstract HealthSystem HealthComponent {  get; set; }

    [SerializeField] public abstract bool DefenseBuffActive { get; set; }
    [SerializeField] public abstract bool AttackBuffActive { get; set; }
    public abstract void Attack(CharacterBattle target, float damage, Action onAttackComplete);
    public void Guard(Action onAttackComplete)
    {
        DefenseBuffActive = true;
        onAttackComplete();
    }

    public int GetMaxHealth()
    {
        return HealthComponent.GetMaxHealth();
    }

    public int GetCurrentHealth()
    {
        return HealthComponent.GetCurrentHealth();
    }
    public void TakeDamage(int damage)
    {
        HealthComponent.Damage(damage);
    }

    public bool IsDead() {
        return HealthComponent.IsDead();
    }
}
