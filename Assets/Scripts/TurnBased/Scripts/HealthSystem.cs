using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class HealthSystem
{

    //public event EventHandler OnHealthChange;
    //public event EventHandler OnDead;

    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;

    public HealthSystem(int maxHP)
    {
        _maxHP = maxHP;
        _currentHP = _maxHP;
    }

    public void SetHealthAmount(int health)
    {
        _currentHP = health;
        // OnHealthChange(this, EventArgs.Empty);
    }

    public int GetCurrentHealth() { return _currentHP; }
    public int GetMaxHealth() { return _maxHP; }

    public void Damage(int amount)
    {
        _currentHP = Mathf.Clamp(_currentHP - amount, 0, _maxHP);
        Debug.Log("CurrentHealth = " + _currentHP.ToString());
        // OnHealthChange(this, EventArgs.Empty);
        if (_currentHP == 0)
        {
            Die();
            Debug.Log("Dead");
        }
    }

    public void Heal(int amount) 
    {
        _currentHP = Mathf.Clamp(_currentHP + amount, 0, _maxHP);
        // OnHealthChange(this, EventArgs.Empty);
    }

    public void Die()
    {
        // OnDead(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return _currentHP == 0;
    }
}
