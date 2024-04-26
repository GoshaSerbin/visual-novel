using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

[System.Serializable]
public class HealthSystem
{

    //public event EventHandler OnHealthChange;
   public UnityEvent OnDead;

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
    }

    public int GetCurrentHealth() { return _currentHP; }
    public int GetMaxHealth() { return _maxHP; }

    public void Damage(int amount)
    {
        _currentHP = Mathf.Clamp(_currentHP - amount, 0, _maxHP);
        if (_currentHP == 0)
        {
            Debug.Log("Dead");
        }
    }

    public void Heal(int amount) 
    {
        _currentHP = Mathf.Clamp(_currentHP + amount, 0, _maxHP);
    }

    public bool IsDead()
    {
        return _currentHP == 0;
    }
}
